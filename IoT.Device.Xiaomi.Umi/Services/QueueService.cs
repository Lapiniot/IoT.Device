using IoT.Protocol.Soap;
using IoT.Protocol.Upnp.Services;
using static System.Globalization.CultureInfo;

namespace IoT.Device.Xiaomi.Umi.Services;

[ServiceSchema(ServiceSchema)]
public sealed class QueueService : SoapActionInvoker
{
    public const string ServiceSchema = "urn:xiaomi-com:service:Queue:1";

    public QueueService(SoapControlEndpoint endpoint) : base(endpoint, ServiceSchema)
    {
    }

    public QueueService(SoapControlEndpoint endpoint, Uri controlUri) :
        base(endpoint, controlUri, ServiceSchema)
    {
    }

    public Task<IReadOnlyDictionary<string, string>> AddUriAsync(uint instanceId, string objectId, uint updateId,
        string enqueuedUri, string enqueuedUriMetaData, uint desiredFirstTrackNumberEnqueued, bool enqueueAsNext,
        CancellationToken cancellationToken)
    {
        return this.InvokeAsync("AddURI", new Dictionary<string, string> {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "ObjectID", objectId },
                { "UpdateID", updateId.ToString(InvariantCulture) },
                { "EnqueuedURI", enqueuedUri },
                { "EnqueuedURIMetaData", enqueuedUriMetaData },
                { "DesiredFirstTrackNumberEnqueued", desiredFirstTrackNumberEnqueued.ToString(InvariantCulture) },
                { "EnqueueAsNext", enqueueAsNext?"true":"false" } }, cancellationToken);
    }

    public Task<IReadOnlyDictionary<string, string>> RemoveAllAsync(uint instanceId, string objectId, uint updateId,
        CancellationToken cancellationToken)
    {
        return this.InvokeAsync("RemoveAll", new Dictionary<string, string> {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "ObjectID", objectId },
                { "UpdateID", updateId.ToString(InvariantCulture) } }, cancellationToken);
    }

    public Task<IReadOnlyDictionary<string, string>> ReorderAsync(uint instanceId, string objectId, uint updateId,
        string trackList, string newPositionList, CancellationToken cancellationToken)
    {
        return this.InvokeAsync("Reorder", new Dictionary<string, string> {
                { "InstanceID", instanceId.ToString(InvariantCulture) },
                { "ObjectID", objectId },
                { "UpdateID", updateId.ToString(InvariantCulture) },
                { "TrackList", trackList },
                { "NewPositionList", newPositionList } }, cancellationToken);
    }
}