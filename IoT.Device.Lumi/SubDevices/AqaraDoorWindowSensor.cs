using IoT.Device.Lumi;
using IoT.Device.Lumi.SubDevices;
using IoT.Device.Metadata;

using static IoT.Device.Metadata.ConnectivityTypes;
using static IoT.Device.Metadata.PowerSource;

[assembly: ExportSubDevice<AqaraDoorWindowSensor>("sensor_magnet.aq2")]

namespace IoT.Device.Lumi.SubDevices;

[ModelID("MCCGQ11LM")]
[PowerSource(CR1632)]
[ConnectivityType(ZigBee)]
public sealed class AqaraDoorWindowSensor : LumiMagnetSensor
{
    internal AqaraDoorWindowSensor(string sid, int id) : base(sid, id) { }

    public override string Model { get; } = "sensor_magnet.aq2";
}