namespace IoT.Device;

public abstract class DeviceFeature<T>(T device)
{
    protected T Device => device;

    public abstract IEnumerable<string> SupportedMethods { get; }

    public abstract IEnumerable<string> SupportedProperties { get; }
}