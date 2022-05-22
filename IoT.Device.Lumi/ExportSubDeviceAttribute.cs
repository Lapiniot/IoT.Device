namespace IoT.Device.Lumi;

public sealed class ExportSubDeviceAttribute<T> : ExportAttribute<LumiSubDevice, T> where T : LumiSubDevice
{
    public ExportSubDeviceAttribute(string model) : base(model, null) { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class ExportSubDeviceAttribute : ExportAttribute<LumiSubDevice, LumiSubDevice>
{
    public ExportSubDeviceAttribute(string model) : base(model, null) { }
}