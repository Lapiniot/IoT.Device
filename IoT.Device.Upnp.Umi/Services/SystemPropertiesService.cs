namespace IoT.Device.Upnp.Umi.Services;

[ExportService(Properties)]
public sealed class SystemPropertiesService(SoapControlEndpoint endpoint, Uri controlUri) : SoapActionInvoker(endpoint, controlUri, Properties), IUpnpService, IUpnpServiceFactory<SystemPropertiesService>
{
    private const string Properties = "urn:xiaomi-com:service:SystemProperties:1";

    public static string ServiceSchema => Properties;

    public async Task<string> GetStringAsync(string variableName, CancellationToken cancellationToken = default) =>
        (await InvokeAsync("GetString", new Dictionary<string, string> { { "VariableName", variableName } }, cancellationToken)
        .ConfigureAwait(false))["StringValue"];

    public static SystemPropertiesService Create(SoapControlEndpoint endpoint, Uri controlUri) => new(endpoint, controlUri);
}