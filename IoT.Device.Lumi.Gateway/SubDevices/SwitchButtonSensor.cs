using System;
using System.Json;
using static System.TimeSpan;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public sealed class SwitchButtonSensor : LumiSubDevice
    {
        private SwitchButtonSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.sensor_switch.v2";

        protected override TimeSpan OfflineTimeout { get; } = FromHours(1);

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);
        }
    }
}