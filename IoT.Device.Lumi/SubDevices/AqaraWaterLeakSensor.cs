using IoT.Device.Metadata;

using static IoT.Device.Metadata.ConnectivityTypes;
using static IoT.Device.Metadata.PowerSource;

namespace IoT.Device.Lumi.SubDevices;

[ExportSubDevice("sensor_wleak.aq1")]
[ModelID("SJCGQ11LM")]
[PowerSource(CR2032)]
[ConnectivityType(ZigBee)]
public sealed class AqaraWaterLeakSensor : LumiSubDeviceWithStatus
{
    internal AqaraWaterLeakSensor(string sid, int id) : base(sid, id) { }

    public override string Model { get; } = "sensor_wleak.aq1";
}