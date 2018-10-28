using System;
using System.Json;
using IoT.Device.Lumi.Interfaces;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class MotionSensorV2 : LumiSubDevice, IProvideStatusInfo
    {
        private int noMotionSeconds;
        private string status;

        private MotionSensorV2(string sid, int id) : base(sid, id)
        {
            status = "nomotion";
        }

        public override string ModelName { get; } = "sensor_motion.v2";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromHours(1);

        public int NoMotionSeconds
        {
            get => noMotionSeconds;
            private set
            {
                if(noMotionSeconds != value)
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
                if(status != value)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);

            if(data.TryGetValue("status", out var s))
            {
                Status = s;
                if(Status == "motion") NoMotionSeconds = 0;
            }

            if(data.TryGetValue("no_motion", out var nm))
            {
                NoMotionSeconds = int.Parse(nm);
                Status = "nomotion";
            }
        }
    }
}