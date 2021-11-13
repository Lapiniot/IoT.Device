namespace IoT.Device.Lumi;

public sealed class ExportSubDeviceAttribute<T> : ExportAttribute<LumiSubDevice, T> where T : LumiSubDevice
{
    public ExportSubDeviceAttribute(string model) : base(model)
    { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class ExportSubDeviceAttribute : ExportAttribute<LumiSubDevice, LumiSubDevice>
{
    public ExportSubDeviceAttribute(string model) : base(model)
    { }
}