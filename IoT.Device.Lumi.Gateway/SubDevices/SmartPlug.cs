using System;
using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public class SmartPlug : LumiSubDevice
    {
        private string status;
        private bool inuse;

        public SmartPlug(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.plug.v1";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromMinutes(10);

        public string Status
        {
            get { return status; }
            set { if (status != value) { status = value; OnPropertyChanged(); } }
        }

        public bool InUse
        {
            get { return inuse; }
            set { if (inuse != value) { inuse = value; OnPropertyChanged(); } }
        }

        protected internal override void Heartbeat(JsonObject data)
        {
            base.Heartbeat(data);

            UpdateState(data);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            if (data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);
            if (data.TryGetValue("status", out var s)) Status = s;
            if (data.TryGetValue("inuse", out var i)) InUse = i == "1";
        }
    }
}