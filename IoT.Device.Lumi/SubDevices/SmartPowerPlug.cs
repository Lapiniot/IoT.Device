using System;
using System.Json;
using static System.TimeSpan;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class SmartPowerPlug : LumiSubDeviceWithStatus
    {
        private bool inUse;
        private decimal loadPower;
        private decimal loadVoltage;
        private decimal powerConsumed;

        private SmartPowerPlug(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "plug.v1";

        protected override TimeSpan OfflineTimeout { get; } = FromMinutes(10);

        public bool InUse
        {
            get => inUse;
            private set => Set(ref inUse, value);
        }

        public decimal LoadVoltage
        {
            get => loadVoltage;
            private set => Set(ref loadVoltage, value);
        }

        public decimal LoadPower
        {
            get => loadPower;
            private set => Set(ref loadPower, value);
        }

        public decimal PowerConsumed
        {
            get => powerConsumed;
            private set => Set(ref powerConsumed, value);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("inuse", out var i))
            {
                InUse = i == "1";
            }

            if(data.TryGetValue("load_voltage", out var lv))
            {
                LoadVoltage = new decimal(lv, 0, 0, false, 3);
            }

            if(data.TryGetValue("load_power", out var lp))
            {
                LoadPower = lp;
            }

            if(data.TryGetValue("power_consumed", out var pc))
            {
                PowerConsumed = pc;
            }
        }
    }
}