using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight
{
    public abstract class YeelightLamp : YeelightDevice
    {
        protected YeelightLamp(YeelightControlEndpoint endpoint) : base(endpoint) { }
    }
}