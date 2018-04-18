using System;
using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public sealed class WindowDoorSensor : LumiSubDevice
    {
        private string status;

        public WindowDoorSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.sensor_magnet.v2";
        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromHours(1);
        public string Status
        {
            get { return status; }
            set { if (status != value) { status = value; OnPropertyChanged(); } }
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
        }
    }
}