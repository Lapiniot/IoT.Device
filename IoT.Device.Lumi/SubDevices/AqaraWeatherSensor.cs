using System.Globalization;
using System.Text.Json;
using IoT.Device.Metadata;
using static System.Text.Json.JsonValueKind;
using static IoT.Device.Metadata.ConnectivityTypes;
using static IoT.Device.Metadata.PowerSource;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("WSDCGQ11LM")]
    [PowerSource(CR2032)]
    [ConnectivityType(ZigBee)]
    public sealed class AqaraWeatherSensor : LumiSubDevice
    {
        private decimal humidity;
        private decimal pressure;
        private decimal temperature;

        internal AqaraWeatherSensor(string sid, int id) : base(sid, id) {}

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

        protected internal override void OnStateChanged(JsonElement state)
        {
            base.OnStateChanged(state);

            if(state.TryGetProperty("temperature", out var value) && value.ValueKind == String)
            {
                Temperature = new decimal(int.Parse(value.GetString(), CultureInfo.InvariantCulture), 0, 0, false, 2);
            }

            if(state.TryGetProperty("humidity", out value) && value.ValueKind == String)
            {
                Humidity = new decimal(int.Parse(value.GetString(), CultureInfo.InvariantCulture), 0, 0, false, 2);
            }

            if(state.TryGetProperty("pressure", out value) && value.ValueKind == String)
            {
                Pressure = new decimal(int.Parse(value.GetString(), CultureInfo.InvariantCulture), 0, 0, false, 3);
            }
        }
    }
}