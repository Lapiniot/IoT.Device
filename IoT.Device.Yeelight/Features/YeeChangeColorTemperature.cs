using System.Globalization;

namespace IoT.Device.Yeelight.Features;

public class YeeChangeColorTemperature : YeelightDeviceFeature
{
    public static readonly Type Type = typeof(YeeChangeColorTemperature);
    private readonly string propCt;
    private readonly string propSetCt;

    public YeeChangeColorTemperature(YeelightDevice device) : this(device, "ct", "set_ct_abx") { }

    protected YeeChangeColorTemperature(YeelightDevice device, string propGet, string propSet) : base(device)
    {
        propCt = propGet;
        propSetCt = propSet;
    }

    public override IEnumerable<string> SupportedMethods => new[] { propSetCt };

    public override IEnumerable<string> SupportedProperties => new[] { propCt };

    public async Task<uint> GetColorTemperatureAsync(CancellationToken cancellationToken = default)
    {
        return uint.Parse(
            (await Device.GetPropertyAsync(propCt, cancellationToken).ConfigureAwait(false)).GetString(),
            CultureInfo.InvariantCulture);
    }

    public Task SetColorTemperatureAsync(uint temperature, CancellationToken cancellationToken = default) =>
        SetColorTemperatureAsync(temperature, Effect.Sudden, 0, cancellationToken);

    public Task SetColorTemperatureAsync(uint temperature, Effect effect = Effect.Smooth,
        int durationMilliseconds = 500, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(propSetCt, new object[] { temperature, effect.ToString().ToLowerInvariant(), durationMilliseconds }, cancellationToken);
}