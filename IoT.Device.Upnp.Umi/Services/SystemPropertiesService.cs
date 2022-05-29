namespace IoT.Device.Upnp.Umi.Services;

[ExportService(Properties)]
public sealed class SystemPropertiesService : SoapActionInvoker, IUpnpService
{
    private const string Properties = "urn:xiaomi-com:service:SystemProperties:1";

    public static string ServiceSchema => Properties;

    public SystemPropertiesService(SoapControlEndpoint endpoint, Uri controlUri) :
        base(endpoint, controlUri, Properties)
    { }

    public SystemPropertiesService(SoapControlEndpoint endpoint) :
        base(endpoint, Properties)
    { }

    public async Task<string> GetStringAsync(string variableName, CancellationToken cancellationToken = default) =>
        (await InvokeAsync("GetString", new Dictionary<string, string>
            {
                { "VariableName", variableName } },
            cancellationToken).ConfigureAwait(false))["StringValue"];
}