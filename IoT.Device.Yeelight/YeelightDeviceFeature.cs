namespace IoT.Device.Yeelight;

public abstract class YeelightDeviceFeature : DeviceFeature<YeelightDevice>
{
    protected static readonly object[] EmptyArgs = Array.Empty<object>();

    protected YeelightDeviceFeature(YeelightDevice device) : base(device)
    {
    }
}