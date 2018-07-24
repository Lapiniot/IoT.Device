using System;
using System.Json;
using IoT.Device.Lumi.Interfaces;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class SmartPlug : LumiSubDevice, IProvideStatusInfo
    {
        private bool inUse;
        private decimal loadPower;
        private decimal loadVoltage;
        private decimal powerConsumed;
        private string status;

        private SmartPlug(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.plug.v1";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromMinutes(10);

        public bool InUse
        {
            get { return inUse; }
            set
            {
                if(inUse != value)
                {
                    inUse = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal LoadVoltage
        {
            get { return loadVoltage; }
            set
            {
                if(loadVoltage != value)
                {
                    loadVoltage = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal LoadPower
        {
            get { return loadPower; }
            set
            {
                if(loadPower != value)
                {
                    loadPower = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal PowerConsumed
        {
            get { return powerConsumed; }
            set
            {
                if(powerConsumed != value)
                {
                    powerConsumed = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                if(status != value)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);
            if(data.TryGetValue("status", out var s)) Status = s;
            if(data.TryGetValue("inuse", out var i)) InUse = i == "1";
            if(data.TryGetValue("load_voltage", out var lv)) LoadVoltage = new decimal(lv, 0, 0, false, 3);
            if(data.TryGetValue("load_power", out var lp)) LoadPower = (decimal)lp;
            if(data.TryGetValue("power_consumed", out var pc)) PowerConsumed = (decimal)pc;
        }
    }
}