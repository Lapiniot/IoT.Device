using System.Text.Json;
using IoT.Device.Lumi.Interfaces;
using static System.Text.Json.JsonValueKind;
using static System.TimeSpan;

namespace IoT.Device.Lumi;

public abstract class LumiSubDevice(string sid, int id) : LumiThing(sid), IProvideBatteryVoltage
{
    private decimal voltage;

    // Battery powered ZigBee devices usually send heartbeats 
    // once per hour to save battery. We give extra 5 seconds
    // after one hour of absent heartbeat before switching
    // to offline state.
    protected override TimeSpan HeartbeatTimeout { get; } = FromHours(1) + FromSeconds(5);

    public decimal Voltage
    {
        get => voltage;
        protected set => Set(ref voltage, value);
    }

    protected internal override void OnStateChanged(JsonElement state)
    {
        if (state.TryGetProperty("voltage", out var value) && value.ValueKind == Number)
        {
            Voltage = new(value.GetInt32(), 0, 0, false, 3);
        }
    }

    public override string ToString() => $"{{\"model\": \"{ModelName}\", \"sid\": \"{Sid}\", \"short_id\": {id}, \"voltage\": {voltage}}}";
}