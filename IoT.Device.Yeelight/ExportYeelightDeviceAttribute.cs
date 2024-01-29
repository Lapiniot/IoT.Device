namespace IoT.Device.Yeelight;

public sealed class ExportYeelightDeviceAttribute<T>(string modelId) :
ExportAttribute<YeelightDevice, T>(modelId, null) where T : YeelightDevice
{ }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class ExportYeelightDeviceAttribute(string modelId) :
ExportAttribute<YeelightDevice, YeelightDevice>(modelId, null)
{ }