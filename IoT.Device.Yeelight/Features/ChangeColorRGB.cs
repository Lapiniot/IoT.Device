using System.Globalization;

namespace IoT.Device.Yeelight.Features;

public class ChangeColorRGB : YeelightDeviceFeature
{
    private readonly string propRGB;
    private readonly string propSetRGB;

    public ChangeColorRGB(YeelightDevice device) : this(device, "rgb", "set_rgb") { }

    protected ChangeColorRGB(YeelightDevice device, string propGet, string propSet) : base(device)
    {
        propRGB = propGet;
        propSetRGB = propSet;
    }

    public override IEnumerable<string> SupportedMethods => [propSetRGB];

    public override IEnumerable<string> SupportedProperties => [propRGB];

    public async Task<uint> GetColorRGBAsync(CancellationToken cancellationToken = default) =>
        uint.Parse(
            (await Device.GetPropertyAsync(propRGB, cancellationToken).ConfigureAwait(false)).GetString()!,
            CultureInfo.InvariantCulture);

    public Task SetColorRGBAsync(uint argb, CancellationToken cancellationToken = default) =>
        SetColorRGBAsync(argb, Effect.Sudden, 0, cancellationToken);

    public Task SetColorRGBAsync(uint rgb, Effect effect = Effect.Smooth,
        int durationMilliseconds = 500, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(propSetRGB, new object[] { rgb, effect.ToString().ToLowerInvariant(), durationMilliseconds }, cancellationToken);
}