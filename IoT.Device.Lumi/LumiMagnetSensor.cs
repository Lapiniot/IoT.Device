using System.Text.Json;
using static System.Text.Json.JsonValueKind;

namespace IoT.Device.Lumi
{
    public abstract class LumiMagnetSensor : LumiSubDeviceWithStatus
    {
        private int noCloseSeconds;

        protected LumiMagnetSensor(string sid, int id) : base(sid, id) {}

        public int NoCloseSeconds
        {
            get => noCloseSeconds;
            private set => Set(ref noCloseSeconds, value);
        }

        protected internal override void OnStateChanged(JsonElement state)
        {
            base.OnStateChanged(state);

            if(state.TryGetProperty("no_close", out var value) &&
               value.ValueKind == String &&
               int.TryParse(value.GetString(), out var seconds))
            {
                NoCloseSeconds = seconds;
            }
        }
    }
}