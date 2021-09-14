namespace IoT.Device.Yeelight.Features;

public class YeeChangeColorRGB : YeelightDeviceFeature
{
    public static readonly Type Type = typeof(YeeChangeColorRGB);

    private readonly string propRGB;
    private readonly string propSetRGB;

    public YeeChangeColorRGB(YeelightDevice device) : this(device, "rgb", "set_rgb") { }

    protected YeeChangeColorRGB(YeelightDevice device, string propGet, string propSet) : base(device)
    {
        propRGB = propGet;
        propSetRGB = propSet;
    }

    public override IEnumerable<string> SupportedMethods => new[] { propSetRGB };

    public override IEnumerable<string> SupportedProperties => new[] { propRGB };

    public async Task<uint> GetColorRGBAsync(CancellationToken cancellationToken = default)
    {
        return (await Device.GetPropertyAsync(propRGB, cancellationToken).ConfigureAwait(false)).GetUInt32();
    }

    public Task SetColorRGBAsync(uint argb, CancellationToken cancellationToken = default)
    {
        return SetColorRGBAsync(argb, Effect.Sudden, 0, cancellationToken);
    }

    public Task SetColorRGBAsync(uint rgb, Effect effect = Effect.Smooth,
        int durationMilliseconds = 500, CancellationToken cancellationToken = default)
    {
        return Device.InvokeAsync(propSetRGB, new object[] { rgb, effect.ToString().ToLowerInvariant(), durationMilliseconds }, cancellationToken);
    }
}