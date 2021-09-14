using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

public abstract class YeelightWhiteLamp : YeelightLamp
{
    protected YeelightWhiteLamp(YeelightControlEndpoint endpoint) : base(endpoint) { }
}