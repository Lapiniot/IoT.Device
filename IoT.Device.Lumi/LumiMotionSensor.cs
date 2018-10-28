using System.Json;

namespace IoT.Device.Lumi
{
    public abstract class LumiMotionSensor : LumiSubDeviceWithStatus
    {
        private int noMotionSeconds;

        protected LumiMotionSensor(string sid, int id) :
            base(sid, id, "nomotion")
        {
        }

        public int NoMotionSeconds
        {
            get => noMotionSeconds;
            private set => Set(ref noMotionSeconds, value);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("status", out _))
            {
                NoMotionSeconds = 0;
            }
            else if(data.TryGetValue("no_motion", out var nomotion))
            {
                NoMotionSeconds = int.Parse(nomotion);
                Status = "nomotion";
            }
        }
    }
}