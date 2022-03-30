using System.Text.Json;
using IoT.Device.Metadata;

namespace IoT.Device.Lumi.SubDevices;

[ExportSubDevice("smoke")]
[ModelID("JTYJ-GD-01LM/BW")]
[PowerSource(PowerSource.CR123A)]
[ConnectivityType(ConnectivityTypes.ZigBee)]
public sealed partial class HonneywellFireSmokeSensor : LumiSubDevice
{
    private bool alarm;

    internal HonneywellFireSmokeSensor(string sid, int id) : base(sid, id) { }

    public bool Alarm
    {
        get => alarm;
        private set => Set(ref alarm, value);
    }

    protected internal override void OnStateChanged(JsonElement state)
    {
        base.OnStateChanged(state);

        if (state.TryGetProperty("alarm", out var value) && value.ValueKind == JsonValueKind.String)
        {
            Alarm = value.GetString() == "1";
        }
    }
}