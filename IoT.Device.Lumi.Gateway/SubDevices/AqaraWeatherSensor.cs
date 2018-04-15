using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public class AqaraWeatherSensor : LumiSubDevice
    {
        public AqaraWeatherSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.weather.v1";

        protected internal override void UpdateState(JsonValue jsonValue)
        {
        }
    }
}