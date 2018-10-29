using System;
using System.Json;
using IoT.Device.Metadata;
using static System.TimeSpan;
using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.Connectivity;

namespace IoT.Device.Lumi.SubDevices
{
    [ModelID("ZNCZ02LM")]
    [PowerSource(Plugged)]
    [Connectivity(ZigBee)]
    public sealed class SmartPowerPlug : LumiSubDeviceWithStatus
    {
        private bool inUse;
        private decimal loadPower;
        private decimal loadVoltage;
        private decimal powerConsumed;

        private SmartPowerPlug(string sid, int id) : base(sid, id)
        {
        }

        public override string Model { get; } = "plug.v1";

        // Plugged devices usually send heartbeat every ~10 minutes.
        // We give extra 10 seconds before transition to offline state.
        protected override TimeSpan HeartbeatTimeout { get; } =
            FromMinutes(10) + FromSeconds(10);

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

        protected internal override void OnStateChanged(JsonObject state)
        {
            base.OnStateChanged(state);

            if(state.TryGetValue("inuse", out var i))
            {
                InUse = i == "1";
            }

            if(state.TryGetValue("load_voltage", out var lv))
            {
                LoadVoltage = new decimal(lv, 0, 0, false, 3);
            }

            if(state.TryGetValue("load_power", out var lp))
            {
                LoadPower = lp;
            }

            if(state.TryGetValue("power_consumed", out var pc))
            {
                PowerConsumed = pc;
            }
        }
    }
}