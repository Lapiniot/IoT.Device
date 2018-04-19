using System;
using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public sealed class HonneywellSmokeSensor : LumiSubDevice
    {
        private bool alarm;

        internal HonneywellSmokeSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.sensor_smoke.v1";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromHours(1);

        public bool Alarm
        {
            get => alarm;
            private set
            {
                if (alarm != value)
                {
                    alarm = value;
                    OnPropertyChanged();
                }
            }
        }

        protected internal override void Heartbeat(JsonObject data)
        {
            base.Heartbeat(data);

            UpdateState(data);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            if (data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);
            if (data.TryGetValue("alarm", out var a)) Alarm = a == "1";
        }
    }
}