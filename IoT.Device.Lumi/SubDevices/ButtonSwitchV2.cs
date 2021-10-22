using IoT.Device.Metadata;
using IoT.Device.Lumi;
using IoT.Device.Lumi.SubDevices;

using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.ConnectivityTypes;

[assembly: ExportSubDevice<ButtonSwitchV2>("switch")]

namespace IoT.Device.Lumi.SubDevices;

[ModelID("WXKG01LM")]
[PowerSource(CR2032)]
[ConnectivityType(ZigBee)]
public sealed class ButtonSwitchV2 : LumiSubDevice
{
    internal ButtonSwitchV2(string sid, int id) : base(sid, id) { }

    public override string Model { get; } = "sensor_switch.v2";
}