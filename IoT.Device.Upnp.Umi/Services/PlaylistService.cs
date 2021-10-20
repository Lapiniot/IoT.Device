using IoT.Protocol.Soap;
using IoT.Protocol.Upnp.Services;
using static System.Globalization.CultureInfo;

namespace IoT.Device.Upnp.Umi.Services;

[ServiceSchema(ServiceSchema)]
public sealed class PlaylistService : SoapActionInvoker
{
    public const string ServiceSchema = "urn:xiaomi-com:service:Playlist:1";

    public PlaylistService(SoapControlEndpoint endpoint, Uri controlUri) :
        base(endpoint, controlUri, ServiceSchema)
    { }

    public PlaylistService(SoapControlEndpoint endpoint) :
        base(endpoint, ServiceSchema)
    { }

    public Task<IReadOnlyDictionary<string, string>> CreateAsync(uint instanceId = 0,
        string title = "", string enqueuedUri = null, string enqueuedUriMetaData = null,
        CancellationToken cancellationToken = default)
    {
        return InvokeAsync("Create", new Dictionary<string, string>() {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "Title", title ?? "" },
                { "EnqueuedURI", enqueuedUri ?? "" },
                { "EnqueuedURIMetaData", enqueuedUriMetaData ?? "" } },
            cancellationToken);
    }

    public Task<IReadOnlyDictionary<string, string>> DeleteAsync(int[] indices, uint instanceId = 0,
        string updateId = "0", CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(indices);
        if(indices.Length == 0) throw new ArgumentException("Must not be empty!", nameof(indices));

        return InvokeAsync("ReorderPlaylists", new Dictionary<string, string>() {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "ObjectID", "PL:" }, { "UpdateID", updateId },
                { "Playlists", string.Join(',', indices) },
                { "NewPositionList", "".PadRight(indices.Length - 1, ',') } },
            cancellationToken);
    }

    public Task<IReadOnlyDictionary<string, string>> RenameAsync(uint instanceId = 0, string objectId = "",
        string title = "", string updateId = "0", CancellationToken cancellationToken = default)
    {
        return InvokeAsync("Rename", new Dictionary<string, string> {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "ObjectID", objectId ?? "" },
                { "Title", title },
                { "UpdateID", updateId } },
            cancellationToken);
    }

    public Task<IReadOnlyDictionary<string, string>> AddUriAsync(uint instanceId = 0, string objectId = "", string updateId = "0",
        string enqueuedUri = null, string enqueuedUriMetaData = null, uint addAtIndex = 4294967295,
        CancellationToken cancellationToken = default)
    {
        return InvokeAsync("AddURI", new Dictionary<string, string>() {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "ObjectID", objectId ?? "" },
                { "UpdateID", updateId },
                { "AddAtIndex", addAtIndex.ToString(InvariantCulture) },
                { "EnqueuedURI", enqueuedUri ?? "" },
                { "EnqueuedURIMetaData", enqueuedUriMetaData ?? "" } },
            cancellationToken);
    }

    public Task<IReadOnlyDictionary<string, string>> RemoveItemsAsync(int[] indices, uint instanceId = 0,
        string objectId = "", string updateId = "0", CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(indices);
        if(indices.Length == 0) throw new ArgumentException("Must not be empty!", nameof(indices));

        return InvokeAsync("Reorder", new Dictionary<string, string>() {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "ObjectID", objectId },
                { "UpdateID", updateId },
                { "TrackList", string.Join(',', indices) },
                { "NewPositionList", "".PadRight(indices.Length - 1, ',') } },
            cancellationToken);
    }
}