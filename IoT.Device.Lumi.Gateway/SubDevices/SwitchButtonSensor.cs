using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public class SwitchButtonSensor : LumiSubDevice
    {
        public SwitchButtonSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.sensor_switch.v2";

        protected internal override void UpdateState(JsonValue jsonValue)
        {
        }
    }
}