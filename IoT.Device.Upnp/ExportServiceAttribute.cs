namespace IoT.Device.Upnp;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class ExportServiceAttribute<T> : ExportAttribute<SoapActionInvoker, T> where T : SoapActionInvoker
{
    public ExportServiceAttribute(string modelId) : base(modelId, null) { }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ExportServiceAttribute : ExportAttribute<SoapActionInvoker, SoapActionInvoker>
{
    public ExportServiceAttribute(string modelId) : base(modelId, null) { }
}