using System;
using System.Json;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class AqaraWaterLeakSensor : LumiSubDevice
    {
        private AqaraWaterLeakSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.sensor_wleak.aq1";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromHours(1);

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);
        }
    }
}