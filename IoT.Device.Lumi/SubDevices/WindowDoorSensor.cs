using System;
using System.Json;
using IoT.Device.Lumi.Interfaces;

namespace IoT.Device.Lumi.SubDevices
{
    public sealed class WindowDoorSensor : LumiSubDevice, IProvideStatusInfo
    {
        private string status;

        private WindowDoorSensor(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.sensor_magnet.v2";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromHours(1);

        public string Status
        {
            get => status;
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
        }
    }
}