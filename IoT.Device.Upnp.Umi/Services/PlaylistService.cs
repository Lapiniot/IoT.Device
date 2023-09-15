namespace IoT.Device.Upnp.Umi.Services;

[ExportService(Playlist)]
public sealed class PlaylistService(SoapControlEndpoint endpoint, Uri controlUri) : SoapActionInvoker(endpoint, controlUri, Playlist), IUpnpService, IUpnpServiceFactory<PlaylistService>
{
    private const string Playlist = "urn:xiaomi-com:service:Playlist:1";

    public static string ServiceSchema => Playlist;

    public Task<IReadOnlyDictionary<string, string>> CreateAsync(uint instanceId = 0,
        string title = "", string enqueuedUri = null, string enqueuedUriMetaData = null,
        CancellationToken cancellationToken = default) =>
        InvokeAsync("Create", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "Title", title ?? "" },
            { "EnqueuedURI", enqueuedUri ?? "" },
            { "EnqueuedURIMetaData", enqueuedUriMetaData ?? "" }
        }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> DeleteAsync([NotNull] int[] indices, uint instanceId = 0,
        string updateId = "0", CancellationToken cancellationToken = default) =>
        indices.Length == 0
            ? throw new ArgumentException("Must not be empty!", nameof(indices))
            : InvokeAsync("ReorderPlaylists", new Dictionary<string, string> {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "ObjectID", "PL:" }, { "UpdateID", updateId },
                { "Playlists", string.Join(',', indices) },
                { "NewPositionList", "".PadRight(indices.Length - 1, ',') }
            }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> RenameAsync(uint instanceId = 0, string objectId = "",
        string title = "", string updateId = "0", CancellationToken cancellationToken = default) =>
        InvokeAsync("Rename", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "ObjectID", objectId ?? "" },
            { "Title", title },
            { "UpdateID", updateId }
        }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> AddUriAsync(uint instanceId = 0, string objectId = "", string updateId = "0",
        string enqueuedUri = null, string enqueuedUriMetaData = null, uint addAtIndex = 4294967295,
        CancellationToken cancellationToken = default) =>
        InvokeAsync("AddURI", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "ObjectID", objectId ?? "" },
            { "UpdateID", updateId },
            { "AddAtIndex", addAtIndex.ToString(InvariantCulture) },
            { "EnqueuedURI", enqueuedUri ?? "" },
            { "EnqueuedURIMetaData", enqueuedUriMetaData ?? "" }
        }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> RemoveItemsAsync([NotNull] int[] indices, uint instanceId = 0,
        string objectId = "", string updateId = "0", CancellationToken cancellationToken = default) =>
        indices.Length == 0
            ? throw new ArgumentException("Must not be empty!", nameof(indices))
            : InvokeAsync("Reorder", new Dictionary<string, string> {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "ObjectID", objectId },
                { "UpdateID", updateId },
                { "TrackList", string.Join(',', indices) },
                { "NewPositionList", "".PadRight(indices.Length - 1, ',') }
            }, cancellationToken);

    public static PlaylistService Create(SoapControlEndpoint endpoint, Uri controlUri) => new(endpoint, controlUri);
}