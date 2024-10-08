using System.Globalization;

namespace IoT.Device.Yeelight.Features;

public class SupportsSaveState(YeelightDevice device) : YeelightDeviceFeature(device)
{
    public override IEnumerable<string> SupportedMethods => ["set_ps", "set_default"];

    public override IEnumerable<string> SupportedProperties => ["save_state"];

    public async Task<SwitchState> GetAutoSaveStateAsync(CancellationToken cancellationToken = default) =>
        (SwitchState)int.Parse(
            (await Device.GetPropertyAsync("save_state", cancellationToken).ConfigureAwait(false)).GetString()!,
            CultureInfo.InvariantCulture);

    public Task SetAutoSaveStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync("set_ps", new[] { "cfg_save_state", ((int)state).ToString(CultureInfo.InvariantCulture) }, cancellationToken);

    public async Task<int> GetInitPowerStateAsync(CancellationToken cancellationToken = default) =>
        int.Parse(
            (await Device.GetPropertyAsync("init_power_opt", cancellationToken).ConfigureAwait(false)).GetString()!,
            CultureInfo.InvariantCulture);

    public Task SetInitPowerStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync("set_ps", new[] { "cfg_init_power", ((int)state + 1).ToString(CultureInfo.InvariantCulture) }, cancellationToken);

    public Task SaveDefaultsAsync(CancellationToken cancellationToken = default) =>
        Device.InvokeAsync("set_default", Array.Empty<object>(), cancellationToken);
}