using System.Globalization;

namespace IoT.Device.Yeelight.Features;

public class YeeProvideLightMode : YeelightDeviceFeature
{
    public YeeProvideLightMode(YeelightDevice device) : base(device) { }

    public override IEnumerable<string> SupportedMethods => Array.Empty<string>();

    public override IEnumerable<string> SupportedProperties => new[] { "active_mode" };

    public async Task<LightMode> GetModeAsync(CancellationToken cancellationToken = default) =>
        (LightMode)int.Parse(
            (await Device.GetPropertyAsync("active_mode", cancellationToken).ConfigureAwait(false)).GetString()!,
            CultureInfo.InvariantCulture);
}