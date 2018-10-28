using System;
using System.Json;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class ButtonSwitchV2 : LumiSubDevice
    {
        private ButtonSwitchV2(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_switch.v2";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromHours(1);

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);
        }
    }
}