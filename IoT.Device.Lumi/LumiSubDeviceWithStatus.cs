using System.Json;
using IoT.Device.Lumi.Interfaces;

namespace IoT.Device.Lumi
{
    public abstract class LumiSubDeviceWithStatus : LumiSubDevice, IProvideStatus
    {
        private string status;

        protected LumiSubDeviceWithStatus(string sid, int id, string defaultStatus = "") :
            base(sid, id)
        {
            status = defaultStatus;
        }

        public string Status
        {
            get => status;
            protected set => Set(ref status, value);
        }

        protected internal override void UpdateState(JsonObject data)
        {
            base.UpdateState(data);

            if(data.TryGetValue("status", out var value))
            {
                Status = value;
            }
        }
    }
}