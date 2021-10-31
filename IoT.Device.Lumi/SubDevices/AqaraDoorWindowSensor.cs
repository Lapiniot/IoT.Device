using IoT.Device.Metadata;

using static IoT.Device.Metadata.ConnectivityTypes;
using static IoT.Device.Metadata.PowerSource;

namespace IoT.Device.Lumi.SubDevices;

[ExportSubDevice("sensor_magnet.aq2")]
[ModelID("MCCGQ11LM")]
[PowerSource(CR1632)]
[ConnectivityType(ZigBee)]
public sealed partial class AqaraDoorWindowSensor : LumiMagnetSensor
{
    internal AqaraDoorWindowSensor(string sid, int id) : base(sid, id) { }
}