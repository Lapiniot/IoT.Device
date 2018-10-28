using System;

namespace IoT.Device.Lumi.SubDevices
{
    public class AqaraDoorWindowSensor : LumiSubDevice
    {
        public AqaraDoorWindowSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_magnet.aq2";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromHours(1);
    }
}