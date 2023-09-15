namespace IoT.Device.Upnp;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class ExportServiceAttribute<T>(string modelId) : ExportAttribute<SoapActionInvoker, T>(modelId, null) where T : SoapActionInvoker { }

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class ExportServiceAttribute(string modelId) : ExportAttribute<SoapActionInvoker, SoapActionInvoker>(modelId, null) { }