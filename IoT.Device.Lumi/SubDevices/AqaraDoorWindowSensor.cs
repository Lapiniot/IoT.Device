using IoT.Device.Metadata;
using static IoT.Device.Metadata.Connectivity;
using static IoT.Device.Metadata.PowerSource;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("MCCGQ11LM")]
    [PowerSource(CR1632)]
    [Connectivity(ZigBee)]
    public sealed class AqaraDoorWindowSensor : LumiMagnetSensor
    {
        private AqaraDoorWindowSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string Model { get; } = "sensor_magnet.aq2";
    }
}