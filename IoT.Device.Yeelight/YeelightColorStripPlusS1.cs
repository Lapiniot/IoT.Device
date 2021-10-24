using IoT.Device.Yeelight;
using IoT.Protocol.Yeelight;


namespace IoT.Device.Yeelight;

[ExportYeelightDevice("strip6")]
public class YeelightColorStripPlusS1 : YeelightColorStripPlus
{
    public YeelightColorStripPlusS1(YeelightControlEndpoint endpoint) : base(endpoint) { }
}