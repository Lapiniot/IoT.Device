using System.Json;
using IoT.Device.Metadata;
using static IoT.Device.Metadata.Connectivity;
using static IoT.Device.Metadata.PowerSource;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("SJCGQ11LM")]
    [PowerSource(CR2032)]
    [Connectivity(ZigBee)]
    public sealed class AqaraWaterLeakSensor : LumiSubDevice
    {
        private AqaraWaterLeakSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string Model { get; } = "sensor_wleak.aq1";

        protected internal override void OnStateChanged(JsonObject state)
        {
            base.OnStateChanged(state);
        }
    }
}