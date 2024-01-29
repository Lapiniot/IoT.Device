namespace IoT.Device.Yeelight;

public abstract class YeelightDeviceFeature(YeelightDevice device) : DeviceFeature<YeelightDevice>(device)
{
    protected static readonly object[] EmptyArgs = [];
}