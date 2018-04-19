using IoT.Device.Lumi.Gateway.Interfaces;

namespace IoT.Device.Lumi.Gateway
{
    public abstract class LumiSubDevice : LumiThing, IProvideVoltageInfo
    {
        private readonly int id;
        private decimal voltage;

        protected LumiSubDevice(string sid, int id) : base(sid)
        {
            this.id = id;
        }

        public decimal Voltage
        {
            get => voltage;
            protected set
            {
                if (voltage != value)
                {
                    voltage = value;
                    OnPropertyChanged();
                }
            }
        }

        public override string ToString()
        {
            return $"{{\"model\": \"{ModelName}\", \"sid\": \"{Sid}\", \"short_id\": {id}}}";
        }
    }
}