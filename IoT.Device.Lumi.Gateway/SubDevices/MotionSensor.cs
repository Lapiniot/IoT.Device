using System;
using System.Json;
using IoT.Device.Lumi.Gateway.Interfaces;
using static System.TimeSpan;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public sealed class MotionSensor : LumiSubDevice, IProvideStatusInfo
    {
        private int noMotionSeconds;
        private string status;

        private MotionSensor(string sid, int id) : base(sid, id)
        {
            status = "nomotion";
        }

        public override string ModelName { get; } = "lumi.sensor_motion.v2";

        protected override TimeSpan OfflineTimeout { get; } = FromHours(1);

        public int NoMotionSeconds
        {
            get => noMotionSeconds;
            private set
            {
                if (noMotionSeconds != value)
                {
                    noMotionSeconds = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Status
        {
            get => status;
            private set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

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