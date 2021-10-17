using IoT.Device.Yeelight;
using IoT.Protocol.Yeelight;

[assembly: ExportYeelightDevice<YeelightColorStripPlusS1>("strip6")]

namespace IoT.Device.Yeelight
{
    public class YeelightColorStripPlusS1 : YeelightColorStripPlus
    {
        public YeelightColorStripPlusS1(YeelightControlEndpoint endpoint) : base(endpoint)
        {
        }
    }
}