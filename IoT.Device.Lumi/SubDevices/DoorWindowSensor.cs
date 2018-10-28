using System.Json;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class DoorWindowSensor : LumiSubDeviceWithStatus
    {
        private DoorWindowSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_magnet.v2";

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);
        }
    }
}