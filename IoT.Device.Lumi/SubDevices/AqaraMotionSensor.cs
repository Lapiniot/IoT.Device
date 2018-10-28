using System;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class AqaraMotionSensor : LumiMotionSensor
    {
        private int lux;

        private AqaraMotionSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_motion.aq2";

        public int Lux
        {
            get => lux;
            set => Set(ref lux, value);
        }

        protected internal override void OnStateChanged(JsonObject state)
        {
            base.OnStateChanged(state);

            if(state.TryGetValue("lux", out var value))
            {
                Lux = value;
            }
        }
    }
}