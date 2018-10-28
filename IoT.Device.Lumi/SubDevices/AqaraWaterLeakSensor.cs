using System.Json;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class AqaraWaterLeakSensor : LumiSubDevice
    {
        private AqaraWaterLeakSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_wleak.aq1";

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);
        }
    }
}