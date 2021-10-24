using IoT.Device.Metadata;

using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.ConnectivityTypes;

namespace IoT.Device.Lumi.SubDevices;

[ExportSubDevice("magnet")]
[ModelID("MCCGQ01LM")]
[PowerSource(CR1632)]
[ConnectivityType(ZigBee)]
public sealed class DoorWindowSensor : LumiMagnetSensor
{
    internal DoorWindowSensor(string sid, int id) : base(sid, id) { }

    public override string Model { get; } = "sensor_magnet.v2";
}