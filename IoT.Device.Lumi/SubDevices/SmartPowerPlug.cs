using System.Text.Json;
using IoT.Device.Metadata;

using static System.Text.Json.JsonValueKind;
using static System.TimeSpan;
using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.ConnectivityTypes;

namespace IoT.Device.Lumi.SubDevices;

[ExportSubDevice("plug")]
[ModelID("ZNCZ02LM")]
[PowerSource(Plugged)]
[ConnectivityType(ZigBee)]
public sealed class SmartPowerPlug : LumiSubDeviceWithStatus
{
    private bool inUse;
    private decimal loadPower;
    private decimal loadVoltage;
    private decimal powerConsumed;

    internal SmartPowerPlug(string sid, int id) : base(sid, id) { }

    public override string Model { get; } = "plug.v1";

    // Plugged devices usually send heartbeat every ~10 minutes.
    // We give extra 10 seconds before transition to offline state.
    protected override TimeSpan HeartbeatTimeout { get; } =
        FromMinutes(10) + FromSeconds(10);

    public bool InUse
    {
        get => inUse;
        private set => Set(ref inUse, value);
    }

    public decimal LoadVoltage
    {
        get => loadVoltage;
        private set => Set(ref loadVoltage, value);
    }

    public decimal LoadPower
    {
        get => loadPower;
        private set => Set(ref loadPower, value);
    }

    public decimal PowerConsumed
    {
        get => powerConsumed;
        private set => Set(ref powerConsumed, value);
    }

    protected internal override void OnStateChanged(JsonElement state)
    {
        base.OnStateChanged(state);

        if(state.TryGetProperty("inuse", out var value) && value.ValueKind == JsonValueKind.String)
        {
            InUse = value.GetString() == "1";
        }

        if(state.TryGetProperty("load_voltage", out value) && value.ValueKind == Number)
        {
            LoadVoltage = new decimal(value.GetInt32(), 0, 0, false, 3);
        }

        if(state.TryGetProperty("load_power", out value) && value.ValueKind == Number)
        {
            LoadPower = value.GetDecimal();
        }

        if(state.TryGetProperty("power_consumed", out value) && value.ValueKind == Number)
        {
            PowerConsumed = value.GetDecimal();
        }
    }
}