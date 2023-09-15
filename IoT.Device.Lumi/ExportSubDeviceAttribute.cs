namespace IoT.Device.Lumi;

public sealed class ExportSubDeviceAttribute<T>(string modelId) : ExportAttribute<LumiSubDevice, T>(modelId, null) where T : LumiSubDevice { }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class ExportSubDeviceAttribute(string modelId) : ExportAttribute<LumiSubDevice, LumiSubDevice>(modelId, null) { }