using System.Json;
using IoT.Device.Metadata;
using static IoT.Device.Metadata.Connectivity;
using static IoT.Device.Metadata.PowerSource;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("RTCGQ11LM")]
    [PowerSource(CR2450)]
    [Connectivity(ZigBee)]
    public sealed class AqaraMotionSensor : LumiMotionSensor
    {
        private int lux;

        private AqaraMotionSensor(string sid, int id) : base(sid, id) {}

        public override string Model { get; } = "sensor_motion.aq2";

        public int Lux
        {
            get => lux;
            set => Set(ref lux, value);
        }

        protected internal override void OnStateChanged(JsonObject state)
        {
            base.OnStateChanged(state);

            if(state.TryGetValue("lux", out var value))
            {
                Lux = value;
            }
        }
    }
}