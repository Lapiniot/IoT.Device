﻿using System.Json;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class ButtonSwitchV2 : LumiSubDevice
    {
        private ButtonSwitchV2(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "sensor_switch.v2";

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);
        }
    }
}