namespace IoT.Device.Upnp.Umi.Services;

[ExportService(Queue)]
public sealed class QueueService : SoapActionInvoker, IUpnpService, IUpnpServiceFactory<QueueService>
{
    private const string Queue = "urn:xiaomi-com:service:Queue:1";

    public static string ServiceSchema => Queue;

    public QueueService(SoapControlEndpoint endpoint, Uri controlUri) :
        base(endpoint, controlUri, Queue)
    { }

    public Task<IReadOnlyDictionary<string, string>> AddUriAsync(uint instanceId, string objectId, uint updateId,
        string enqueuedUri, string enqueuedUriMetaData, uint desiredFirstTrackNumberEnqueued, bool enqueueAsNext,
        CancellationToken cancellationToken) =>
        InvokeAsync("AddURI", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "ObjectID", objectId },
            { "UpdateID", updateId.ToString(InvariantCulture) },
            { "EnqueuedURI", enqueuedUri },
            { "EnqueuedURIMetaData", enqueuedUriMetaData },
            { "DesiredFirstTrackNumberEnqueued", desiredFirstTrackNumberEnqueued.ToString(InvariantCulture) },
            { "EnqueueAsNext", enqueueAsNext?"true":"false" } }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> RemoveAllAsync(uint instanceId, string objectId, uint updateId,
        CancellationToken cancellationToken) =>
        InvokeAsync("RemoveAll", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "ObjectID", objectId },
            { "UpdateID", updateId.ToString(InvariantCulture) } }, cancellationToken);

    public Task<IReadOnlyDictionary<string, string>> ReorderAsync(uint instanceId, string objectId, uint updateId,
        string trackList, string newPositionList, CancellationToken cancellationToken) =>
        InvokeAsync("Reorder", new Dictionary<string, string> {
            { "InstanceID", instanceId.ToString(InvariantCulture) },
            { "ObjectID", objectId },
            { "UpdateID", updateId.ToString(InvariantCulture) },
            { "TrackList", trackList },
            { "NewPositionList", newPositionList } }, cancellationToken);

    public static QueueService Create(SoapControlEndpoint endpoint, Uri controlUri) => new(endpoint, controlUri);
}