using System.Globalization;

namespace IoT.Device.Yeelight.Features;

public class YeeChangeBrightness : YeelightDeviceFeature
{
    public static readonly Type Type = typeof(YeeChangeBrightness);

    private readonly string propBright;
    private readonly string propSetBright;

    public YeeChangeBrightness(YeelightDevice device) : this(device, "bright", "set_bright") { }

    protected YeeChangeBrightness(YeelightDevice device, string propGet, string propSet) : base(device)
    {
        propBright = propGet;
        propSetBright = propSet;
    }

    public override IEnumerable<string> SupportedMethods => new[] { propSetBright };

    public override IEnumerable<string> SupportedProperties => new[] { propBright };

    public async Task<uint> GetBrightnessAsync(CancellationToken cancellationToken = default)
    {
        return uint.Parse(
            (await Device.GetPropertyAsync(propBright, cancellationToken).ConfigureAwait(false)).GetString(),
            CultureInfo.InvariantCulture);
    }

    public Task SetBrightnessAsync(uint brightness, CancellationToken cancellationToken = default)
    {
        return SetBrightnessAsync(brightness, Effect.Sudden, 0, cancellationToken);
    }

    public Task SetBrightnessAsync(uint brightness, Effect effect = Effect.Smooth,
        int durationMilliseconds = 500, CancellationToken cancellationToken = default)
    {
        return Device.InvokeAsync(propSetBright, new object[] { brightness, effect.ToString().ToLowerInvariant(), durationMilliseconds }, cancellationToken);
    }
}