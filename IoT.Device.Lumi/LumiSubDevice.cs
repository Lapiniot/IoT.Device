using System;
using System.Json;
using IoT.Device.Lumi.Interfaces;
using static System.TimeSpan;

namespace IoT.Device.Lumi
{
    public abstract class LumiSubDevice : LumiThing, IProvideVoltageInfo
    {
        private readonly int id;
        private decimal voltage;

        protected LumiSubDevice(string sid, int id) : base(sid)
        {
            this.id = id;
        }

        protected override TimeSpan OfflineTimeout { get; } = FromHours(1);

        public decimal Voltage
        {
            get => voltage;
            protected set => Set(ref voltage, value);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("voltage", out var value))
            {
                Voltage = new decimal(value, 0, 0, false, 3);
            }
        }

        public override string ToString()
        {
            return $"{{\"model\": \"{ModelName}\", \"sid\": \"{Sid}\", \"short_id\": {id}, \"voltage\": {voltage}}}";
        }
    }
}