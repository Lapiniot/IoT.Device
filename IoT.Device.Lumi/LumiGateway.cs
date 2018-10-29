using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using IoT.Device.Lumi.SubDevices;
using IoT.Device.Metadata;
using IoT.Protocol.Lumi;
using static System.TimeSpan;
using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.Connectivity;
using Cache = IoT.Device.Container<IoT.Device.Lumi.ExportSubDeviceAttribute, IoT.Device.Lumi.LumiSubDevice>;

namespace IoT.Device.Lumi
{
    [ModelID("DGNWG02LM")]
    [PowerSource(Plugged)]
    [Connectivity(WiFi24 | ZigBee)]
    public sealed class LumiGateway : LumiThing, IObserver<JsonObject>
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
        protected override TimeSpan HeartbeatTimeout { get; } =
            FromSeconds(10) + FromSeconds(2);

        void IObserver<JsonObject>.OnCompleted()
        {
            // Empty by design
        }

        void IObserver<JsonObject>.OnError(Exception error)
        {
            // Empty by design
        }

        void IObserver<JsonObject>.OnNext(JsonObject message)
        {
            if(message.TryGetValue("sid", out var sid) && message.TryGetValue("cmd", out var command) &&
               message.TryGetValue("data", out var v) && JsonValue.Parse(v) is JsonObject data)
            {
                switch((string)command)
                {
                    case "heartbeat":
                    {
                        if(sid == Sid)
                        {
                            OnHeartbeat(data);
                            Token = message["token"];
                        }
                        else if(children.TryGetValue(sid, out var device))
                        {
                            device.OnHeartbeat(data);
                        }
                    }
                        break;
                    case "report":
                    {
                        if(sid == Sid)
                        {
                            OnStateChanged(data);
                        }
                        else if(children.TryGetValue(sid, out var device))
                        {
                            device.OnStateChanged(data);
                        }
                    }
                        break;
                }
            }
        }

        public Task<JsonObject> InvokeAsync(string command, string sid = null,
            CancellationToken cancellationToken = default)
        {
            return client.InvokeAsync(command, sid ?? Sid, cancellationToken);
        }

        public async Task<LumiSubDevice[]> GetChildrenAsync(CancellationToken cancellationToken = default)
        {
            var j = await InvokeAsync("get_id_list", Sid, cancellationToken).ConfigureAwait(false);

            var sids = ((JsonArray)JsonValue.Parse(j["data"]) ?? throw new InvalidOperationException())
                .Select(s => (string)s).ToArray();

            await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                var adds = sids.Except(children.Keys).ToArray();
                var removes = children.Keys.Except(sids).ToArray();

                foreach(var sid in adds)
                {
                    var info = await InvokeAsync("read", sid, cancellationToken).ConfigureAwait(false);

                    if(JsonValue.Parse(info["data"]) is JsonObject data && !children.TryGetValue(sid, out var device))
                    {
                        var id = (int)info["short_id"];
                        var deviceModel = info["model"];
                        device = Cache.CreateInstance((string)deviceModel, sid, id) ?? new GenericSubDevice(sid, id);
                        device.OnStateChanged(data);
                        children.Add(sid, device);
                    }
                }

                foreach(var sid in removes)
                {
                    if(children.TryGetValue(sid, out var device))
                    {
                        children.Remove(sid);

                        device.Dispose();
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }

            return children.Values.ToArray();
        }

        protected internal override void OnStateChanged(JsonObject state)
        {
            if(state.TryGetValue("rgb", out var rgb))
            {
                RgbValue = rgb;
            }

            if(state.TryGetValue("illumination", out var value))
            {
                Illumination = value;
            }
        }

        #region Overrides of LumiThing

        protected override void OnConnect()
        {
            base.OnConnect();

            client.Connect();
            listener.Connect();
        }

        protected override void OnClose()
        {
            base.OnClose();

            client.Close();
            listener.Close();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if(disposing)
            {
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
    }
}