using System.Json;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class AqaraDoorWindowSensor : LumiSubDeviceWithStatus
    {
        private int noCloseSeconds;

        private AqaraDoorWindowSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_magnet.aq2";

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