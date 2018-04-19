using System;
using System.Json;
using static System.TimeSpan;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public sealed class AqaraWeatherSensor : LumiSubDevice
    {
        private decimal humidity;
        private decimal pressure;
        private decimal temperature;

        private AqaraWeatherSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.weather.v1";

        protected override TimeSpan OfflineTimeout { get; } = FromHours(1);

        public decimal Temperature
        {
            get => temperature;
            private set
            {
                if (temperature != value)
                {
                    temperature = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Humidity
        {
            get => humidity;
            private set
            {
                if (humidity != value)
                {
                    humidity = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Pressure
        {
            get => pressure;
            private set
            {
                if (pressure != value)
                {
                    pressure = value;
                    OnPropertyChanged();
                }
            }
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if (data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);

            if (data.TryGetValue("temperature", out var t)) Temperature = new decimal(t, 0, 0, false, 2);

            if (data.TryGetValue("humidity", out var h)) Humidity = new decimal(h, 0, 0, false, 2);

            if (data.TryGetValue("pressure", out var p)) Pressure = new decimal(p, 0, 0, false, 3);
        }
    }
}