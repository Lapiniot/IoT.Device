using System;

namespace IoT.Device.Lumi.SubDevices
{
    public class AqaraCubeController : LumiSubDevice
    {
        public AqaraCubeController(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_cube.aqgl01";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromHours(1);
    }
}