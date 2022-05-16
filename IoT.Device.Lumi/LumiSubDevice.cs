using System.Text.Json;
using IoT.Device.Lumi.Interfaces;
using static System.Text.Json.JsonValueKind;
using static System.TimeSpan;

namespace IoT.Device.Lumi;

public abstract class LumiSubDevice : LumiThing, IProvideBatteryVoltage
{
    private readonly int id;
    private decimal voltage;

    protected LumiSubDevice(string sid, int id) : base(sid) => this.id = id;

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
            Voltage = new decimal(value.GetInt32(), 0, 0, false, 3);
        }
    }

    public override string ToString() => $"{{\"model\": \"{ModelName}\", \"sid\": \"{Sid}\", \"short_id\": {id}, \"voltage\": {voltage}}}";
}