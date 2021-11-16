using IoT.Protocol.Soap;

namespace IoT.Device.Upnp.Umi.Services;

[ExportService(ServiceSchema)]
public sealed class SystemPropertiesService : SoapActionInvoker
{
    public const string ServiceSchema = "urn:xiaomi-com:service:SystemProperties:1";

    public SystemPropertiesService(SoapControlEndpoint endpoint, Uri controlUri) :
        base(endpoint, controlUri, ServiceSchema)
    { }

    public SystemPropertiesService(SoapControlEndpoint endpoint) :
        base(endpoint, ServiceSchema)
    { }

    public async Task<string> GetStringAsync(string variableName, CancellationToken cancellationToken = default)
    {
        return (await InvokeAsync("GetString", new Dictionary<string, string>() {
                { "VariableName", variableName } },
            cancellationToken).ConfigureAwait(false))["StringValue"];
    }
}