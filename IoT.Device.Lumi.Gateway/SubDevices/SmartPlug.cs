using System;
using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public sealed class SmartPlug : LumiSubDevice
    {
        private bool inuse;
        private decimal loadPower;
        private decimal loadVoltage;
        private decimal powerConsumed;
        private string status;

        internal SmartPlug(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.plug.v1";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromMinutes(10);

        public string Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool InUse
        {
            get => inuse;
            set
            {
                if (inuse != value)
                {
                    inuse = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal LoadVoltage
        {
            get => loadVoltage;
            set
            {
                if (loadVoltage != value)
                {
                    loadVoltage = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal LoadPower
        {
            get => loadPower;
            set
            {
                if (loadPower != value)
                {
                    loadPower = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal PowerConsumed
        {
            get => powerConsumed;
            set
            {
                if (powerConsumed != value)
                {
                    powerConsumed = value;
                    OnPropertyChanged();
                }
            }
        }

        protected internal override void Heartbeat(JsonObject data)
        {
            base.Heartbeat(data);

            UpdateState(data);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            if (data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);
            if (data.TryGetValue("status", out var s)) Status = s;
            if (data.TryGetValue("inuse", out var i)) InUse = i == "1";
            if (data.TryGetValue("load_voltage", out var lv)) LoadVoltage = new decimal(lv, 0, 0, false, 3);
            if (data.TryGetValue("load_power", out var lp)) LoadPower = (decimal) lp;
            if (data.TryGetValue("power_consumed", out var pc)) PowerConsumed = (decimal) pc;
        }
    }
}