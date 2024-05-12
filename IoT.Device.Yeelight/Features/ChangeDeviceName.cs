namespace IoT.Device.Yeelight.Features;

public class ChangeDeviceName(YeelightDevice device) : YeelightDeviceFeature(device)
{
    public override IEnumerable<string> SupportedMethods => ["set_name"];

    public override IEnumerable<string> SupportedProperties => ["name"];

    public async Task<string> GetNameAsync(CancellationToken cancellationToken = default) =>
        (await Device.GetPropertyAsync("name", cancellationToken).ConfigureAwait(false)).GetString();

    public Task SetNameAsync(string name, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync("set_name", new[] { name }, cancellationToken);
}