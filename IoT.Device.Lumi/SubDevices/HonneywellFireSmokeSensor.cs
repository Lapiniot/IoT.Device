using System.Text.Json;
using IoT.Device.Metadata;
using static System.Text.Json.JsonValueKind;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("JTYJ-GD-01LM/BW")]
    [PowerSource(PowerSource.CR123A)]
    [Connectivity(Connectivity.ZigBee)]
    public sealed class HonneywellFireSmokeSensor : LumiSubDevice
    {
        private bool alarm;

        internal HonneywellFireSmokeSensor(string sid, int id) : base(sid, id) {}

        public override string Model { get; } = "sensor_smoke.v1";

        public bool Alarm
        {
            get => alarm;
            private set => Set(ref alarm, value);
        }

        protected internal override void OnStateChanged(JsonElement state)
        {
            base.OnStateChanged(state);

            if(state.TryGetProperty("alarm", out var value) && value.ValueKind == String)
            {
                Alarm = value.GetString() == "1";
            }
        }
    }
}