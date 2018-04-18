using System;
using System.ComponentModel;
using System.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Lumi.Gateway
{
    public abstract class LumiSubDevice : LumiThing
    {
        private int id;
        private decimal voltage;
        public decimal Voltage
        {
            get { return voltage; }
            protected set { if (voltage != value) { voltage = value; OnPropertyChanged(); } }
        }

        public LumiSubDevice(string sid, int id) : base(sid) => this.id = id;

        public override string ToString()
        {
            return $"{{\"model\": \"{ModelName}\", \"sid\": \"{Sid}\", \"short_id\": {id}}}";
        }
    }
}