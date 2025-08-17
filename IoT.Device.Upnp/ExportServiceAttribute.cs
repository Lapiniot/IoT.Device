namespace IoT.Device.Upnp;

public sealed class ExportServiceAttribute(string modelId) :
    ExportAttribute<SoapActionInvoker, SoapActionInvoker>(modelId, null)
{ }