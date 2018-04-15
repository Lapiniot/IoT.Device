using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public class HonneywellSmokeSensor : LumiSubDevice
    {
        public HonneywellSmokeSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.sensor_smoke.v1";

        protected internal override void UpdateState(JsonValue jsonValue)
        {
        }
    }
}