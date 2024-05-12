using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

[ExportYeelightDevice("strip6")]
public partial class ColorStripPlusS1(YeelightControlEndpoint endpoint) : ColorStripPlus(endpoint) { }