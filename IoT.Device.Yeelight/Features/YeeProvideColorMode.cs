using System.Globalization;

namespace IoT.Device.Yeelight.Features;

public class YeeProvideColorMode : YeelightDeviceFeature
{
    private readonly string property;

    protected YeeProvideColorMode(YeelightDevice device, string colorModeProp) : base(device) => property = colorModeProp;

    public YeeProvideColorMode(YeelightDevice device) : this(device, "color_mode") { }

    public override IEnumerable<string> SupportedMethods => [];

    public override IEnumerable<string> SupportedProperties => [property];

    public async Task<ColorMode> GetColorModeAsync(CancellationToken cancellationToken = default) =>
        (ColorMode)int.Parse(
            (await Device.GetPropertyAsync(property, cancellationToken).ConfigureAwait(false)).GetString()!,
            CultureInfo.InvariantCulture);
}