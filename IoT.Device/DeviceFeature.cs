namespace IoT.Device;

public abstract class DeviceFeature<T>
{
    protected DeviceFeature(T device)
    {
        Device = device;
    }

    protected T Device { get; }

    public abstract IEnumerable<string> SupportedMethods { get; }

    public abstract IEnumerable<string> SupportedProperties { get; }
}