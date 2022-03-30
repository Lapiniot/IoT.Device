using System.Text.Json;
using IoT.Device.Metadata;

using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.ConnectivityTypes;

namespace IoT.Device.Lumi.SubDevices;

[ExportSubDevice("sensor_cube.aqgl01")]
[ModelID("MFKZQ01LM")]
[PowerSource(CR2450)]
[ConnectivityType(ZigBee)]
public sealed partial class AqaraCubeController : LumiSubDevice
{
    private int rotateAngle;
    private int rotateDuration;

    internal AqaraCubeController(string sid, int id) : base(sid, id) { }

    public int RotateAngle
    {
        get => rotateAngle;
        private set => Set(ref rotateAngle, value);
    }

    public int RotateDuration
    {
        get => rotateDuration;
        private set => Set(ref rotateDuration, value);
    }

    protected internal override void OnStateChanged(JsonElement state)
    {
        base.OnStateChanged(state);

        if (!state.TryGetProperty("rotate", out var value) || value.ValueKind != JsonValueKind.String) return;

        var str = value.GetString();
        if (str == null) return;

        var i = str.IndexOf(',', StringComparison.InvariantCulture);

        if (i <= 0 || i >= str.Length - 1 ||
           !int.TryParse(str.AsSpan(0, i), out var angle) ||
           !int.TryParse(str[(i + 1)..], out var duration))
        {
            return;
        }

        RotateAngle = (int)Math.Round(angle * 3.6);
        RotateDuration = duration;
    }
}