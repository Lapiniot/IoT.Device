using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

[ExportYeelightDevice("strip6")]
public partial class YeelightColorStripPlusS1(YeelightControlEndpoint endpoint) : YeelightColorStripPlus(endpoint)
{
}