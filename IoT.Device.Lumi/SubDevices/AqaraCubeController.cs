using System;
using System.Text.Json;
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
        private int rotateAngle;
        private int rotateDuration;

        internal AqaraCubeController(string sid, int id) : base(sid, id) {}

        public override string Model { get; } = "sensor_cube.aqgl01";

        public int RotateAngle
        {
            get => rotateAngle;
            private set => Set(ref rotateAngle, value);
        }

        public int RotateDuration
        {
            get => rotateDuration;
            private set => Set(ref rotateDuration, value);
        }

        protected internal override void OnStateChanged(JsonElement state)
        {
            base.OnStateChanged(state);

            if(!state.TryGetProperty("rotate", out var value) || value.ValueKind != JsonValueKind.String) return;

            var str = value.GetString();
            var i = str.IndexOf(',');

            if(i <= 0 || i >= str.Length - 1 ||
               !int.TryParse(str.Substring(0, i), out var angle) ||
               !int.TryParse(str.Substring(i + 1), out var duration))
            {
                return;
            }

            RotateAngle = (int)Math.Round(angle * 3.6);
            RotateDuration = duration;
        }
    }
}