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

        protected internal override void OnStateChanged(JsonObject state)
        {
            base.OnStateChanged(state);

            // Special value "iam" is usual device online
            // report when sensor's test button is pressed
            if(state.TryGetValue("status", out var value) && value != "iam")
            {
                Status = value;
            }
        }
    }
}