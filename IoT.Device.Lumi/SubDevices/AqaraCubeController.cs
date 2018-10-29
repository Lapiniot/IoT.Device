using System.Json;
using IoT.Device.Metadata;
using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.Connectivity;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("MFKZQ01LM")]
    [PowerSource(CR2450)]
    [Connectivity(ZigBee)]
    public sealed class AqaraCubeController : LumiSubDevice
    {
        private decimal rotate;

        private AqaraCubeController(string sid, int id) : base(sid, id)
        {
        }

        public override string Model { get; } = "sensor_cube.aqgl01";

        public decimal Rotate
        {
            get => rotate;
            set => Set(ref rotate, value);
        }

        protected internal override void OnStateChanged(JsonObject state)
        {
            base.OnStateChanged(state);

            if(state.TryGetValue("rotate", out var angle))
            {
                Rotate = angle;
            }
        }
    }
}