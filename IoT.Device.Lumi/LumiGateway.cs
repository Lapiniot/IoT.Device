using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using IoT.Device.Lumi.SubDevices;
using IoT.Protocol.Lumi;
using IoT.Protocol.Lumi.Interfaces;
using Cache = IoT.Device.Container<
    IoT.Device.Lumi.ExportSubDeviceAttribute,
    IoT.Device.Lumi.LumiSubDevice>;

namespace IoT.Device.Lumi
{
    public sealed class LumiGateway : LumiThing, ILumiEventListener
    {
        private readonly Dictionary<string, LumiSubDevice> children;
        private readonly SemaphoreSlim semaphore;
        private LumiControlEndpoint client;
        private bool disposed;
        private int illumination;
        private LumiEventListener listener;
        private int rgbValue;

        public LumiGateway(IPAddress address, ushort port, string sid) : base(sid)
        {
            semaphore = new SemaphoreSlim(1, 1);
            children = new Dictionary<string, LumiSubDevice>();
            client = new LumiControlEndpoint(new IPEndPoint(address, port));
            listener = new LumiEventListener(this, new IPEndPoint(IPAddress.Parse("224.0.0.50"), port));
        }

        public string Token { get; private set; }

        public int RgbValue
        {
            get { return rgbValue; }
            private set
            {
                if(rgbValue != value)
                {
                    rgbValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Illumination
        {
            get { return illumination; }
            private set
            {
                if(illumination != value)
                {
                    illumination = value;
                    OnPropertyChanged();
                }
            }
        }

        public override string ModelName { get; } = "gateway";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromSeconds(15);

        public void OnReportMessage(string sid, JsonObject data, JsonObject message)
        {
            if(sid == Sid && message.TryGetValue("model", out var model) && model == "gateway")
            {
                UpdateState(data);
            }
            else if(children.TryGetValue(sid, out var device))
            {
                device.UpdateState(data);
            }
        }

        public void OnHeartbeatMessage(string sid, JsonObject data, JsonObject message)
        {
            if(sid == Sid && message.TryGetValue("model", out var model) && model == "gateway")
            {
                Token = message["token"];
            }
            else if(children.TryGetValue(sid, out var device))
            {
                device.UpdateState(data);
            }
        }

        public void Connect()
        {
            client.Connect();
            listener.Connect();
        }

        public void Close()
        {
            client.Close();
            listener.Close();
        }

        public Task<JsonObject> InvokeAsync(string command, string sid = null,
            CancellationToken cancellationToken = default)
        {
            return client.InvokeAsync(command, sid ?? Sid, cancellationToken);
        }

        public async Task<LumiSubDevice[]> GetChildrenAsync(CancellationToken cancellationToken = default)
        {
            var j = await InvokeAsync("get_id_list", Sid, cancellationToken);

            var sids = ((JsonArray) JsonValue.Parse(j["data"]) ?? throw new InvalidOperationException())
                .Select(s => (string) s).ToArray();

            await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                var adds = sids.Except(children.Keys).ToArray();
                var removes = children.Keys.Except(sids).ToArray();

                foreach(var sid in adds)
                {
                    var info = await InvokeAsync("read", sid, cancellationToken);

                    if(JsonValue.Parse(info["data"]) is JsonObject data && !children.TryGetValue(sid, out var device))
                    {
                        var id = (int) info["short_id"];
                        var deviceModel = info["model"];
                        device = Cache.CreateInstance((string) deviceModel, sid, id) ?? new GenericSubDevice(sid, id);
                        device.UpdateState(data);
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

        protected internal override void UpdateState(JsonObject data)
        {
            if(data.TryGetValue("rgb", out var rgb)) RgbValue = rgb;

            if(data.TryGetValue("illumination", out var i)) Illumination = i;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if(!disposed)
            {
                if(disposing)
                {
                    client.Dispose();
                    client = null;

                    listener.Dispose();
                    listener = null;

                    semaphore.Dispose();

                    foreach(var c in children) c.Value.Dispose();
                }

                disposed = true;
            }
        }
    }
}