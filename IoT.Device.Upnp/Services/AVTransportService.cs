namespace IoT.Device.Upnp.Services;

[CLSCompliant(false)]
[ExportService(AVTransport)]
public sealed class AVTransportService : SoapActionInvoker, IUpnpService, IUpnpServiceFactory<AVTransportService>
{
    public static string ServiceSchema => AVTransport;

    public AVTransportService(SoapControlEndpoint endpoint, Uri controlUri) :
        base(endpoint, controlUri, AVTransport)
    { }

    public Task<IReadOnlyDictionary<string, string>> GetMediaInfoAsync(uint instanceId = 0, CancellationToken cancellationToken = default) =>
        InvokeAsync("GetMediaInfo", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) }
        }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> GetPositionInfoAsync(uint instanceId = 0, CancellationToken cancellationToken = default) =>
        InvokeAsync("GetPositionInfo", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) }
        }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> GetTransportInfoAsync(uint instanceId = 0, CancellationToken cancellationToken = default) =>
        InvokeAsync("GetTransportInfo", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) }
        }, cancellationToken);

    public Task SetAVTransportUriAsync(uint instanceId = 0, string currentUri = null, string currentUriMetaData = null,
        CancellationToken cancellationToken = default) =>
        InvokeAsync("SetAVTransportURI", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "CurrentURI", currentUri },
            { "CurrentURIMetaData", currentUriMetaData }
        }, cancellationToken);

    public Task SetNextAVTransportUriAsync(uint instanceId = 0, string nextUri = null, string nextUriMetaData = null,
        CancellationToken cancellationToken = default) =>
        InvokeAsync("SetNextAVTransportURI", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "NextURI", nextUri },
            { "NextURIMetaData", nextUriMetaData }
        }, cancellationToken);

    public Task StopAsync(uint instanceId = 0, CancellationToken cancellationToken = default) =>
        InvokeAsync("Stop", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) }
        }, cancellationToken);

    public Task PlayAsync(uint instanceId = 0, string speed = "1", CancellationToken cancellationToken = default) =>
        InvokeAsync("Play", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "Speed", speed }
        }, cancellationToken);

    public Task PauseAsync(uint instanceId = 0, CancellationToken cancellationToken = default) =>
        InvokeAsync("Pause", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) }
        }, cancellationToken);

    public Task NextAsync(uint instanceId = 0, CancellationToken cancellationToken = default) =>
        InvokeAsync("Next", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) }
        }, cancellationToken);

    public Task PreviousAsync(uint instanceId = 0, CancellationToken cancellationToken = default) =>
        InvokeAsync("Previous", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) }
        }, cancellationToken);

    public Task SeekAsync(uint instanceId = 0, string seekMode = "ABS_TIME", string target = null, CancellationToken cancellationToken = default) =>
        InvokeAsync("Seek", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "Unit", seekMode },
            { "Target", target }
        }, cancellationToken);

    public Task SetPlayModeAsync(uint instanceId = 0, string newPlayMode = "NORMAL", CancellationToken cancellationToken = default) =>
        InvokeAsync("SetPlayMode", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "NewPlayMode", newPlayMode }
        }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> GetCurrentTransportActionsAsync(uint instanceId = 0, CancellationToken cancellationToken = default) =>
        InvokeAsync("GetCurrentTransportActions", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) }
        }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> GetTransportSettingsAsync(uint instanceId = 0, CancellationToken cancellationToken = default) =>
        InvokeAsync("GetTransportSettings", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) }
        }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> GetDeviceCapabilitiesAsync(uint instanceId = 0, CancellationToken cancellationToken = default) =>
        InvokeAsync("GetDeviceCapabilities", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) }
        }, cancellationToken);

    public static AVTransportService Create(SoapControlEndpoint endpoint, Uri controlUri) => new(endpoint, controlUri);
}