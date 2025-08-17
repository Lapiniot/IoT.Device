namespace IoT.Device.Yeelight;

public sealed class ExportYeelightDeviceAttribute(string modelId) :
    ExportAttribute<YeelightDevice, YeelightDevice>(modelId, null)
{ }