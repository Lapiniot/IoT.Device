using System.Globalization;

namespace IoT.Device.Yeelight.Features;

public class ChangeColorHSV : YeelightDeviceFeature
{
    private readonly string propHue;
    private readonly string propSaturation;
    private readonly string propSetHSV;

    public ChangeColorHSV(YeelightDevice device) : this(device, "hue", "sat", "set_hsv") { }

    protected ChangeColorHSV(YeelightDevice device, string propHueGet, string propSaturationGet, string propHSVSet) : base(device)
    {
        propHue = propHueGet;
        propSaturation = propSaturationGet;
        propSetHSV = propHSVSet;
    }

    public override IEnumerable<string> SupportedMethods => [propSetHSV];

    public override IEnumerable<string> SupportedProperties => [propHue, propSaturation];

    public async Task<uint> GetHueAsync(CancellationToken cancellationToken = default) =>
        uint.Parse(
            (await Device.GetPropertyAsync(propHue, cancellationToken).ConfigureAwait(false)).GetString(),
            CultureInfo.InvariantCulture);

    public async Task<uint> GetSaturationAsync(CancellationToken cancellationToken = default) =>
        uint.Parse(
            (await Device.GetPropertyAsync(propSaturation, cancellationToken).ConfigureAwait(false)).GetString(),
            CultureInfo.InvariantCulture);

    public Task SetColorHSVAsync(uint hsv, CancellationToken cancellationToken = default) =>
        SetColorHSVAsync(hsv, Effect.Sudden, 0, cancellationToken);

    public Task SetColorHSVAsync(uint hsv, Effect effect = Effect.Smooth, int durationMilliseconds = 500, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(propSetHSV, new object[] { hsv, effect.ToString().ToLowerInvariant(), durationMilliseconds }, cancellationToken);
}