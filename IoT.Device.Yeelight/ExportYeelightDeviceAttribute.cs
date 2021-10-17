namespace IoT.Device.Yeelight;

public sealed class ExportYeelightDeviceAttribute<T> : ExportAttribute<T> where T : YeelightDevice
{
    public ExportYeelightDeviceAttribute(string model) : base(model)
    { }
}