using IoT.Device.Lumi;
using IoT.Device.Lumi.SubDevices;
using IoT.Device.Metadata;

using static IoT.Device.Metadata.ConnectivityTypes;
using static IoT.Device.Metadata.PowerSource;

[assembly: ExportSubDevice<AqaraWaterLeakSensor>("sensor_wleak.aq1")]

namespace IoT.Device.Lumi.SubDevices;

[ModelID("SJCGQ11LM")]
[PowerSource(CR2032)]
[ConnectivityType(ZigBee)]
public sealed class AqaraWaterLeakSensor : LumiSubDeviceWithStatus
{
    internal AqaraWaterLeakSensor(string sid, int id) : base(sid, id) { }

    public override string Model { get; } = "sensor_wleak.aq1";
}