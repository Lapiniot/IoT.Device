namespace IoT.Device.Lumi.SubDevices
{
    public sealed class DoorWindowSensor : LumiMagnetSensor
    {
        private DoorWindowSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_magnet.v2";
    }
}