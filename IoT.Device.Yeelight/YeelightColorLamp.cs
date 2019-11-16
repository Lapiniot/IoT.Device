using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight
{
    public abstract class YeelightColorLamp : YeelightWhiteLamp
    {
        protected YeelightColorLamp(YeelightControlEndpoint endpoint) : base(endpoint) { }
    }
}