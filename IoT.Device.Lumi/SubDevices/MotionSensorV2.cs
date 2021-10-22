using IoT.Device.Metadata;
using IoT.Device.Lumi;
using IoT.Device.Lumi.SubDevices;

using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.ConnectivityTypes;

[assembly: ExportSubDevice<MotionSensorV2>("motion")]

namespace IoT.Device.Lumi.SubDevices;

[ModelID("RTCGQ01LM")]
[PowerSource(CR2450)]
[ConnectivityType(ZigBee)]
public sealed class MotionSensorV2 : LumiMotionSensor
{
    internal MotionSensorV2(string sid, int id) : base(sid, id) { }

    public override string Model { get; } = "sensor_motion.v2";
}