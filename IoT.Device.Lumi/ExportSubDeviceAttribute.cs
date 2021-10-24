namespace IoT.Device.Lumi;

public sealed class ExportSubDeviceAttribute<T> : ExportAttribute<T> where T : LumiSubDevice
{
    public ExportSubDeviceAttribute(string model) : base(model)
    { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class ExportSubDeviceAttribute : ExportAttribute<LumiSubDevice>
{
    public ExportSubDeviceAttribute(string model) : base(model)
    { }
}