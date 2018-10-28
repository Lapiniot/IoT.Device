using System.Json;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class HonneywellFireSmokeSensor : LumiSubDevice
    {
        private bool alarm;

        private HonneywellFireSmokeSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_smoke.v1";

        public bool Alarm
        {
            get => alarm;
            private set => Set(ref alarm, value);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("alarm", out var a))
            {
                Alarm = a == "1";
            }
        }
    }
}