using IoT.Device.Metadata;
using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.Connectivity;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("RTCGQ01LM")]
    [PowerSource(CR2450)]
    [Connectivity(ZigBee)]
    public sealed class MotionSensorV2 : LumiMotionSensor
    {
        private MotionSensorV2(string sid, int id) : base(sid, id) {}

        public override string Model { get; } = "sensor_motion.v2";
    }
}