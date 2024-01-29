using System.Globalization;

namespace IoT.Device.Yeelight.Features;

public class YeeProvideLightMode(YeelightDevice device) : YeelightDeviceFeature(device)
{
    public override IEnumerable<string> SupportedMethods => [];

    public override IEnumerable<string> SupportedProperties => ["active_mode"];

    public async Task<LightMode> GetModeAsync(CancellationToken cancellationToken = default) =>
        (LightMode)int.Parse(
            (await Device.GetPropertyAsync("active_mode", cancellationToken).ConfigureAwait(false)).GetString()!,
            CultureInfo.InvariantCulture);
}