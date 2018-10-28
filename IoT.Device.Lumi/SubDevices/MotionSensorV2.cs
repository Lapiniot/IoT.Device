namespace IoT.Device.Lumi.SubDevices
{
    public sealed class MotionSensorV2 : LumiMotionSensor
    {
        private MotionSensorV2(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_motion.v2";
    }
}