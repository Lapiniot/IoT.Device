using IoT.Device.Metadata;
using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.Connectivity;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("WXKG01LM")]
    [PowerSource(CR2032)]
    [Connectivity(ZigBee)]
    public sealed class ButtonSwitchV2 : LumiSubDevice
    {
        private ButtonSwitchV2(string sid, int id) : base(sid, id)
        {
        }

        public override string Model { get; } = "sensor_switch.v2";
    }
}