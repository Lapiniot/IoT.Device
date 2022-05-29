namespace IoT.Device.Upnp.Services;

[ExportService(ConnectionManager)]
public class ConnectionManagerService : SoapActionInvoker, IUpnpService, IUpnpServiceFactory<ConnectionManagerService>
{
    public static string ServiceSchema => ConnectionManager;

    public ConnectionManagerService(SoapControlEndpoint endpoint, Uri controlUri) :
        base(endpoint, controlUri, ConnectionManager)
    { }

    public Task<IReadOnlyDictionary<string, string>> GetProtocolInfoAsync(CancellationToken cancellationToken = default) =>
        InvokeAsync("GetProtocolInfo", null, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> GetCurrentConnectionInfoAsync(string connectionId, CancellationToken cancellationToken = default) =>
        InvokeAsync("GetCurrentConnectionInfo", new Dictionary<string, string> { { "ConnectionID", connectionId } }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> GetCurrentConnectionIDsAsync(CancellationToken cancellationToken = default) =>
        InvokeAsync("GetCurrentConnectionIDs", null, cancellationToken);

    public static ConnectionManagerService Create(SoapControlEndpoint endpoint, Uri controlUri) => new(endpoint, controlUri);
}