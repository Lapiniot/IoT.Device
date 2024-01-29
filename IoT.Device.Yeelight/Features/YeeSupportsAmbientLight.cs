using System.Globalization;

namespace IoT.Device.Yeelight.Features;

public class YeeSupportsAmbientLight(YeelightDevice device) : YeelightDeviceFeature(device)
{

    public override IEnumerable<string> SupportedMethods => ["set_ps"];

    public override IEnumerable<string> SupportedProperties => ["bg_proact"];

    public async Task<SwitchState> GetProactiveModeAsync(CancellationToken cancellationToken = default) =>
        (SwitchState)int.Parse(
            (await Device.GetPropertyAsync("bg_proact", cancellationToken).ConfigureAwait(false)).GetString()!,
            CultureInfo.InvariantCulture);

    /// <summary>
    /// Sets whether ambient light will be switched on/off with main light
    /// </summary>
    /// <param name="state">On/Off option</param>
    /// <param name="cancellationToken">Token for external cancellation</param>
    /// <returns></returns>
    public Task SetProactiveModeAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync("set_ps", new[] { "cfg_bg_proact", ((int)state).ToString(CultureInfo.InvariantCulture) }, cancellationToken);
}