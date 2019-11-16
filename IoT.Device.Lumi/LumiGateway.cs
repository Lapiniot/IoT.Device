using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IoT.Device.Lumi.SubDevices;
using IoT.Device.Metadata;
using IoT.Protocol.Lumi;
using static System.Text.Json.JsonSerializer;
using static System.Text.Json.JsonValueKind;
using static System.TimeSpan;
using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.Connectivity;
using Cache = IoT.Device.Container<IoT.Device.Lumi.ExportSubDeviceAttribute, IoT.Device.Lumi.LumiSubDevice>;

namespace IoT.Device.Lumi
{
    [ModelID("DGNWG02LM")]
    [PowerSource(Plugged)]
    [Connectivity(WiFi24 | ZigBee)]
    public sealed class LumiGateway : LumiThing, IConnectedObject, IObserver<JsonElement>
    {
        private readonly Dictionary<string, LumiSubDevice> children;
        private readonly LumiControlEndpoint client;
        private readonly LumiEventListener listener;
        private readonly SemaphoreSlim semaphore;
        private readonly IDisposable subscription;
        private int illumination;
        private int rgbValue;

        public LumiGateway(IPAddress address, ushort port, string sid) : base(sid)
        {
            semaphore = new SemaphoreSlim(1, 1);
            children = new Dictionary<string, LumiSubDevice>();
            client = new LumiControlEndpoint(new IPEndPoint(address, port));
            listener = new LumiEventListener(new IPEndPoint(IPAddress.Parse("224.0.0.50"), port));
            subscription = listener.Subscribe(this);
        }

        public string Token { get; private set; }

        public int RgbValue
        {
            get => rgbValue;
            private set => Set(ref rgbValue, value);
        }

        public int Illumination
        {
            get => illumination;
            private set => Set(ref illumination, value);
        }

        public override string Model { get; } = "gateway";

        // Gateway sends heartbeats every 10 seconds.
        // We give extra 2 seconds to the timeout value.
        protected override TimeSpan HeartbeatTimeout { get; } = FromSeconds(10) + FromSeconds(2);

        public bool IsConnected { get; private set; }

        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            if(!IsConnected)
            {
                await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

                try
                {
                    if(!IsConnected)
                    {
                        await client.ConnectAsync(cancellationToken).ConfigureAwait(false);
                        await listener.ConnectAsync(cancellationToken).ConfigureAwait(false);
                        IsConnected = true;
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }

        public async Task DisconnectAsync()
        {
            if(IsConnected)
            {
                await semaphore.WaitAsync().ConfigureAwait(false);

                try
                {
                    if(IsConnected)
                    {
                        await client.DisconnectAsync().ConfigureAwait(false);
                        await listener.DisconnectAsync().ConfigureAwait(false);
                    }
                }
                finally
                {
                    IsConnected = false;

                    semaphore.Release();
                }
            }
        }

        public Task<JsonElement> InvokeAsync(string command, string sid = null, CancellationToken cancellationToken = default)
        {
            return client.InvokeAsync(command, sid ?? Sid, cancellationToken);
        }

        public async IAsyncEnumerable<LumiSubDevice> GetChildrenAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var json = await InvokeAsync("get_id_list", Sid, cancellationToken).ConfigureAwait(false);

            var data = Deserialize<JsonElement>(json.GetProperty("data").GetString());

            var sids = data.EnumerateArray().Select(a => a.GetString()).ToArray();

            await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                var adds = sids.Except(children.Keys).ToArray();
                var removes = children.Keys.Except(sids).ToArray();

                foreach(var sid in removes)
                {
                    if(children.TryGetValue(sid, out var device))
                    {
                        children.Remove(sid);

                        device.Dispose();
                    }
                }

                foreach(var (_, value) in children)
                {
                    yield return value;
                }

                foreach(var sid in adds)
                {
                    var info = await InvokeAsync("read", sid, cancellationToken).ConfigureAwait(false);

                    if(info.TryGetProperty("data", out var d) && !children.TryGetValue(sid, out var device))
                    {
                        var id = info.GetProperty("short_id").GetInt32();
                        var deviceModel = info.GetProperty("model").GetString();
                        device = Cache.CreateInstance(deviceModel, sid, id) ?? new GenericSubDevice(sid, id);
                        device.OnStateChanged(Deserialize<JsonElement>(d.GetString()));
                        children.Add(sid, device);
                        yield return device;
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        protected internal override void OnStateChanged(JsonElement state)
        {
            if(state.TryGetProperty("rgb", out var value) && value.ValueKind == Number)
            {
                RgbValue = value.GetInt32();
            }

            if(state.TryGetProperty("illumination", out value) && value.ValueKind == Number)
            {
                Illumination = value.GetInt32();
            }
        }

        #region Overrides of LumiThing

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if(disposing)
            {
                var _ = client.DisposeAsync();
                listener.DisposeAsync();
                semaphore?.Dispose();

                subscription.Dispose();

                foreach(var c in children)
                {
                    c.Value.Dispose();
                }

                children.Clear();
            }
        }

        #endregion

        #region Implementation of IObserver<in JsonElement>

        void IObserver<JsonElement>.OnCompleted() {}

        void IObserver<JsonElement>.OnError(Exception error) {}

        void IObserver<JsonElement>.OnNext(JsonElement message)
        {
            if(message.TryGetProperty("sid", out var sid) && message.TryGetProperty("cmd", out var command) &&
               message.TryGetProperty("data", out var v) && Deserialize<JsonElement>(v.GetString()) is JsonElement data)
            {
                var key = sid.GetString();
                switch(command.GetString())
                {
                    case "heartbeat":
                    {
                        if(key == Sid)
                        {
                            OnHeartbeat(data);
                            Token = message.GetProperty("token").GetString();
                        }
                        else if(children.TryGetValue(key, out var device))
                        {
                            device.OnHeartbeat(data);
                        }
                    }
                        break;
                    case "report":
                    {
                        if(key == Sid)
                        {
                            OnStateChanged(data);
                        }
                        else if(children.TryGetValue(key, out var device))
                        {
                            device.OnStateChanged(data);
                        }
                    }
                        break;
                }
            }
        }

        #endregion
    }
}