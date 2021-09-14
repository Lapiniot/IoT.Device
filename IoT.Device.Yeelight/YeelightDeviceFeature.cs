namespace IoT.Device.Yeelight;

public abstract class YeelightDeviceFeature
{
    protected static readonly object[] EmptyArgs = Array.Empty<object>();

    protected YeelightDeviceFeature(YeelightDevice device)
    {
        Device = device;
    }

    protected YeelightDevice Device { get; }

    public abstract IEnumerable<string> SupportedMethods { get; }

    public abstract IEnumerable<string> SupportedProperties { get; }
}