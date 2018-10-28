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

        protected internal override void OnStateChanged(JsonObject state)
        {
            base.OnStateChanged(state);

            if(state.TryGetValue("alarm", out var a))
            {
                Alarm = a == "1";
            }
        }
    }
}