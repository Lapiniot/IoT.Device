using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

public abstract class YeelightLamp(YeelightControlEndpoint endpoint) : YeelightDevice(endpoint)
{
}