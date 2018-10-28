namespace IoT.Device.Lumi.SubDevices
{
    public sealed class AqaraDoorWindowSensor : LumiMagnetSensor
    {
        private AqaraDoorWindowSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_magnet.aq2";
    }
}