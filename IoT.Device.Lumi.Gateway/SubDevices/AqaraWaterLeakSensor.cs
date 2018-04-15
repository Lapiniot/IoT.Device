using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public class AqaraWaterLeakSensor : LumiSubDevice
    {
        public AqaraWaterLeakSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.sensor_wleak.aq1";

        protected internal override void UpdateState(JsonValue jsonValue)
        {
        }
    }
}