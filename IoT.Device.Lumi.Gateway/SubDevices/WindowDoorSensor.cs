using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public class WindowDoorSensor : LumiSubDevice
    {
        public WindowDoorSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.sensor_magnet.v2";

        protected internal override void UpdateState(JsonValue jsonValue)
        {
        }
    }
}