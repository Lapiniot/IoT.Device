namespace IoT.Device.Yeelight;

public sealed class ExportYeelightDeviceAttribute<T> : ExportAttribute<YeelightDevice, T> where T : YeelightDevice
{
    public ExportYeelightDeviceAttribute(string model) : base(model) { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class ExportYeelightDeviceAttribute : ExportAttribute<YeelightDevice, YeelightDevice>
{
    public ExportYeelightDeviceAttribute(string model) : base(model) { }
}