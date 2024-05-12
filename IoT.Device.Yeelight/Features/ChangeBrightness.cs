using System.Globalization;

namespace IoT.Device.Yeelight.Features;

public class ChangeBrightness : YeelightDeviceFeature
{
    private readonly string propBright;
    private readonly string propSetBright;

    public ChangeBrightness(YeelightDevice device) : this(device, "bright", "set_bright") { }

    protected ChangeBrightness(YeelightDevice device, string propGet, string propSet) : base(device)
    {
        propBright = propGet;
        propSetBright = propSet;
    }

    public override IEnumerable<string> SupportedMethods => [propSetBright];

    public override IEnumerable<string> SupportedProperties => [propBright];

    public async Task<uint> GetBrightnessAsync(CancellationToken cancellationToken = default) =>
        uint.Parse(
            (await Device.GetPropertyAsync(propBright, cancellationToken).ConfigureAwait(false)).GetString(),
            CultureInfo.InvariantCulture);

    public Task SetBrightnessAsync(uint brightness, CancellationToken cancellationToken = default) =>
        SetBrightnessAsync(brightness, Effect.Sudden, 0, cancellationToken);

    public Task SetBrightnessAsync(uint brightness, Effect effect = Effect.Smooth,
        int durationMilliseconds = 500, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(propSetBright, new object[] { brightness, effect.ToString().ToLowerInvariant(), durationMilliseconds }, cancellationToken);
}