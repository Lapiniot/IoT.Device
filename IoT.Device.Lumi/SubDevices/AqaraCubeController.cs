using System;

namespace IoT.Device.Lumi.SubDevices
{
    public class AqaraCubeController : LumiSubDevice
    {
        private decimal rotate;

        private AqaraCubeController(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_cube.aqgl01";

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