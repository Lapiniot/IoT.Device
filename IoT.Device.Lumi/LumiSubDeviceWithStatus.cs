using System.Text.Json;
using IoT.Device.Lumi.Interfaces;
using static System.Text.Json.JsonValueKind;

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

        protected internal override void OnStateChanged(JsonElement state)
        {
            base.OnStateChanged(state);

            // Special value "iam" is usual device online
            // report when sensor's test button is pressed
            if(!state.TryGetProperty("status", out var value) || value.ValueKind != JsonValueKind.String) return;

            var str = value.GetString();

            if(str != "iam") Status = str;
        }
    }
}