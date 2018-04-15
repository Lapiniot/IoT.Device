using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public class MotionSensor : LumiSubDevice
    {


        public MotionSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.sensor_motion.v2";


        protected internal override void UpdateState(JsonObject data)
        {
            if (data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);
        }
    }
}