using System;
using System.Json;
using IoT.Device.Lumi.Gateway.Interfaces;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public class MotionSensor : LumiSubDevice, IProvideStatusInfo
    {
        private string status;
        private int noMotionSeconds;

        public MotionSensor(string sid, int id) : base(sid, id)
        {
            status = "nomotion";
        }

        public override string ModelName { get; } = "lumi.sensor_motion.v2";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromHours(1);

        public string Status
        {
            get { return status; }
            private set { if (status != value) { status = value; OnPropertyChanged(); } }
        }

        public int NoMotionSeconds
        {
            get { return noMotionSeconds; }
            private set { if (noMotionSeconds != value) { noMotionSeconds = value; OnPropertyChanged(); } }
        }

        protected internal override void Heartbeat(JsonObject data)
        {
            base.Heartbeat(data);

            UpdateState(data);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            if (data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);
            if (data.TryGetValue("status", out var s))
            {
                Status = s;
                if (Status == "motion") NoMotionSeconds = 0;
            }
            if (data.TryGetValue("no_motion", out var nm))
            {
                NoMotionSeconds = int.Parse(nm);
                Status = "nomotion";
            }
        }
    }
}