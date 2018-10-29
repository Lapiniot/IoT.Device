using System.Json;
using IoT.Device.Metadata;
using static IoT.Device.Metadata.Connectivity;
using static IoT.Device.Metadata.PowerSource;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("WSDCGQ11LM")]
    [PowerSource(CR2032)]
    [Connectivity(ZigBee)]
    public sealed class AqaraWeatherSensor : LumiSubDevice
    {
        private decimal humidity;
        private decimal pressure;
        private decimal temperature;

        private AqaraWeatherSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string Model { get; } = "weather.v1";

        public decimal Temperature
        {
            get => temperature;
            private set => Set(ref temperature, value);
        }

        public decimal Humidity
        {
            get => humidity;
            private set => Set(ref humidity, value);
        }

        public decimal Pressure
        {
            get => pressure;
            private set => Set(ref pressure, value);
        }

        protected internal override void OnStateChanged(JsonObject state)
        {
            base.OnStateChanged(state);

            if(state.TryGetValue("temperature", out var t))
            {
                Temperature = new decimal(t, 0, 0, false, 2);
            }

            if(state.TryGetValue("humidity", out var h))
            {
                Humidity = new decimal(h, 0, 0, false, 2);
            }

            if(state.TryGetValue("pressure", out var p))
            {
                Pressure = new decimal(p, 0, 0, false, 3);
            }
        }
    }
}