using System.Json;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class MotionSensorV2 : LumiSubDeviceWithStatus
    {
        private int noMotionSeconds;

        private MotionSensorV2(string sid, int id) : base(sid, id)
        {
            Status = "nomotion";
        }

        public override string ModelName { get; } = "sensor_motion.v2";

        public int NoMotionSeconds
        {
            get => noMotionSeconds;
            private set => Set(ref noMotionSeconds, value);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(Status == "motion")
            {
                NoMotionSeconds = 0;
            }

            if(data.TryGetValue("no_motion", out var nomotion))
            {
                NoMotionSeconds = int.Parse(nomotion);
                Status = "nomotion";
            }
        }
    }
}