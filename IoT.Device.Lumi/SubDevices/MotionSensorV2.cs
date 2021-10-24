using IoT.Device.Metadata;

using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.ConnectivityTypes;

namespace IoT.Device.Lumi.SubDevices;

[ExportSubDevice("motion")]
[ModelID("RTCGQ01LM")]
[PowerSource(CR2450)]
[ConnectivityType(ZigBee)]
public sealed class MotionSensorV2 : LumiMotionSensor
{
    internal MotionSensorV2(string sid, int id) : base(sid, id) { }

    public override string Model { get; } = "sensor_motion.v2";
}