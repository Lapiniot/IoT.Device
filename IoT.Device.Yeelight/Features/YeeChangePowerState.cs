namespace IoT.Device.Yeelight.Features;

public class YeeChangePowerState : YeelightDeviceFeature
{
    public static readonly Type Type = typeof(YeeChangePowerState);
    private readonly string propGetPower;
    private readonly string propSetPower;
    private readonly string propSetToggle;

    public YeeChangePowerState(YeelightDevice device) : this(device, "power", "set_power", "toggle") { }

    protected YeeChangePowerState(YeelightDevice device, string propGetPower, string propSetPower, string propSetToggle) :
        base(device)
    {
        this.propGetPower = propGetPower;
        this.propSetPower = propSetPower;
        this.propSetToggle = propSetToggle;
    }

    public override IEnumerable<string> SupportedMethods => new[] { propSetPower };

    public override IEnumerable<string> SupportedProperties => new[] { propGetPower };

    public Task<SwitchState> GetPowerStateAsync(CancellationToken cancellationToken = default)
    {
        return Device.GetPropertyAsync<SwitchState>(propGetPower, cancellationToken);
    }

    public Task SetPowerStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
    {
        return SetPowerStateAsync(state, Effect.Sudden, 0, ColorMode.Normal, cancellationToken);
    }

    public Task SetPowerStateAsync(SwitchState state = SwitchState.On, Effect effect = Effect.Smooth,
        uint durationMilliseconds = 500, ColorMode mode = ColorMode.Normal,
        CancellationToken cancellationToken = default)
    {
        var args = new object[] { state.ToString().ToLowerInvariant(), effect.ToString().ToLowerInvariant(), durationMilliseconds, (int)mode };

        return Device.InvokeAsync(propSetPower, args, cancellationToken);
    }

    public Task ToggleAsync(CancellationToken cancellationToken = default)
    {
        return Device.InvokeAsync(propSetToggle, Array.Empty<object>(), cancellationToken);
    }
}