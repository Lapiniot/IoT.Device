namespace IoT.Device.Lumi;

public sealed class ExportSubDeviceAttribute(string modelId) :
    ExportAttribute<LumiSubDevice, LumiSubDevice>(modelId, null)
{ }