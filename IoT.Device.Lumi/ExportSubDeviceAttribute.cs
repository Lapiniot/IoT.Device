namespace IoT.Device.Lumi;

public sealed class ExportSubDeviceAttribute<T> : ExportAttribute<T> where T : LumiSubDevice
{
    public ExportSubDeviceAttribute(string model) : base(model)
    { }
}