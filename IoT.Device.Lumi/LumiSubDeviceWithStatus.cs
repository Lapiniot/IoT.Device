using System.Text.Json;
using IoT.Device.Lumi.Interfaces;

namespace IoT.Device.Lumi;

public abstract class LumiSubDeviceWithStatus(string sid, int id, string defaultStatus = "") : LumiSubDevice(sid, id), IProvideStatus
{
    public string Status
    {
        get => defaultStatus;
        protected set => Set(ref defaultStatus, value);
    }

    protected internal override void OnStateChanged(JsonElement state)
    {
        base.OnStateChanged(state);

        // Special value "iam" is usual device online
        // report when sensor's test button is pressed
        if (!state.TryGetProperty("status", out var value) || value.ValueKind != JsonValueKind.String) return;

        var str = value.GetString();

        if (str != "iam") Status = str;
    }
}