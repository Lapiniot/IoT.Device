﻿using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using IoT.Device.Lumi.SubDevices;
using IoT.Protocol.Lumi;
using Cache = IoT.Device.Container<
    IoT.Device.Lumi.ExportSubDeviceAttribute,
    IoT.Device.Lumi.LumiSubDevice>;
using ILumiObserver = System.IObserver<(string Command, string Sid, System.Json.JsonObject Data, System.Json.JsonObject Message)>;

namespace IoT.Device.Lumi
{
    public sealed class LumiGateway : LumiThing, ILumiObserver
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
            get => illumination;
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

        void ILumiObserver.OnCompleted()
        {
            // Empty by design
        }

        void ILumiObserver.OnError(Exception error)
        {
            // Empty by design
        }

        void ILumiObserver.OnNext((string Command, string Sid, JsonObject Data, JsonObject Message) value)
        {
            switch(value.Command)
            {
                case "heartbeat":
                    OnHeartbeatMessage(value.Sid, value.Data, value.Message);
                    break;
                case "report":
                    OnReportMessage(value.Sid, value.Data, value.Message);
                    break;
            }
        }

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