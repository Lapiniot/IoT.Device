using System.Json;

namespace IoT.Device.Lumi
{
    public abstract class LumiMagnetSensor : LumiSubDeviceWithStatus
    {
        private int noCloseSeconds;

        protected LumiMagnetSensor(string sid, int id) : base(sid, id)
        {
        }

        public int NoCloseSeconds
        {
            get => noCloseSeconds;
            private set => Set(ref noCloseSeconds, value);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("no_close", out var value) &&
               int.TryParse(value, out var seconds))
            {
                NoCloseSeconds = seconds;
            }
        }
    }
}