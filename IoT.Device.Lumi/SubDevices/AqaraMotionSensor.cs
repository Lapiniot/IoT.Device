using System.Text.Json;
using IoT.Device.Lumi;
using IoT.Device.Lumi.SubDevices;
using IoT.Device.Metadata;

using static System.Text.Json.JsonValueKind;
using static IoT.Device.Metadata.ConnectivityTypes;
using static IoT.Device.Metadata.PowerSource;

[assembly: ExportSubDevice<AqaraMotionSensor>("sensor_motion.aq2")]

namespace IoT.Device.Lumi.SubDevices;

[ModelID("RTCGQ11LM")]
[PowerSource(CR2450)]
[ConnectivityType(ZigBee)]
public sealed class AqaraMotionSensor : LumiMotionSensor
{
    private int lux;

    internal AqaraMotionSensor(string sid, int id) : base(sid, id) { }

    public override string Model { get; } = "sensor_motion.aq2";

    public int Lux
    {
        get => lux;
        set => Set(ref lux, value);
    }

    protected internal override void OnStateChanged(JsonElement state)
    {
        base.OnStateChanged(state);

        if(state.TryGetProperty("lux", out var value) && value.ValueKind == Number)
        {
            Lux = value.GetInt32();
        }
    }
}