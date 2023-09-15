using System.Text.Json;

namespace IoT.Device.Lumi;

public abstract class LumiMagnetSensor(string sid, int id) : LumiSubDeviceWithStatus(sid, id)
{
    private int noCloseSeconds;

    public int NoCloseSeconds
    {
        get => noCloseSeconds;
        private set => Set(ref noCloseSeconds, value);
    }

    protected internal override void OnStateChanged(JsonElement state)
    {
        base.OnStateChanged(state);

        if (state.TryGetProperty("no_close", out var value) &&
           value.ValueKind == JsonValueKind.String &&
           int.TryParse(value.GetString(), out var seconds))
        {
            NoCloseSeconds = seconds;
        }
    }
}