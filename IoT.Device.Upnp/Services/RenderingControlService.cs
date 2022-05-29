namespace IoT.Device.Upnp.Services;

[CLSCompliant(false)]
[ExportService(RenderingControl)]
public sealed class RenderingControlService : SoapActionInvoker, IUpnpService
{
    public static string ServiceSchema => RenderingControl;

    public RenderingControlService(SoapControlEndpoint endpoint, Uri controlUri) :
        base(endpoint, controlUri, RenderingControl)
    { }

    public RenderingControlService(SoapControlEndpoint endpoint) :
        base(endpoint, RenderingControl)
    { }

    public Task<IReadOnlyDictionary<string, string>> GetVolumeAsync(uint instanceId, CancellationToken cancellationToken = default) =>
        InvokeAsync("GetVolume", new Dictionary<string, string>
        {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "Channel", "Master" } }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> SetVolumeAsync(uint instanceId, uint volume, CancellationToken cancellationToken = default) =>
        InvokeAsync("SetVolume", new Dictionary<string, string>
            {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "Channel", "Master" },
                { "DesiredVolume", volume.ToString(InvariantCulture) } },
            cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> GetMuteAsync(uint instanceId, CancellationToken cancellationToken = default) =>
        InvokeAsync("GetMute", new Dictionary<string, string>
            {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "Channel", "Master" } },
            cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> SetMuteAsync(uint instanceId, bool mute, CancellationToken cancellationToken = default) =>
        InvokeAsync("SetMute", new Dictionary<string, string>
            {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "Channel", "Master" },
                { "DesiredMute", mute ? "true" : "false" } },
            cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> GetPresetsAsync(uint instanceId, CancellationToken cancellationToken = default) =>
        InvokeAsync("ListPresets", new Dictionary<string, string>
            {
                { "InstanceID", instanceId.ToString(InvariantCulture) } },
            cancellationToken);
}