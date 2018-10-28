using System;

namespace IoT.Device.Lumi.SubDevices
{
    public class AqaraMotionSensor : LumiSubDevice
    {
        public AqaraMotionSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_motion.aq2";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromHours(1);
    }
}