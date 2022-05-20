namespace IoT.Device.Yeelight.Features;

public class YeeChangeDeviceName : YeelightDeviceFeature
{
    public YeeChangeDeviceName(YeelightDevice device) : base(device) { }

    public override IEnumerable<string> SupportedMethods => new[] { "set_name" };

    public override IEnumerable<string> SupportedProperties => new[] { "name" };

    public async Task<string> GetNameAsync(CancellationToken cancellationToken = default) =>
        (await Device.GetPropertyAsync("name", cancellationToken).ConfigureAwait(false)).GetString();

    public Task SetNameAsync(string name, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync("set_name", new[] { name }, cancellationToken);
}