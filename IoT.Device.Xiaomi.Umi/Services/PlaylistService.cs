using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Soap;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class PlaylistService : SoapActionInvoker
    {
        internal PlaylistService(UmiSpeakerDevice parent) : base(parent.Endpoint,
            $"{parent.DeviceId}-MR/xiaomi.com-Playlist-1/control",
            "urn:xiaomi-com:service:Playlist:1")
        {
        }

        public Task<IDictionary<string, string>> CreateAsync(uint instanceId = 0,
            string title = "", string enqueuedUri = null, string enqueuedUriMetaData = null,
            CancellationToken cancellationToken = default)
        {
            return InvokeAsync("Create", cancellationToken, ("InstanceID", instanceId), ("Title", title ?? ""),
                ("EnqueuedURI", enqueuedUri ?? ""), ("EnqueuedURIMetaData", enqueuedUriMetaData ?? ""));
        }

        public Task<IDictionary<string, string>> AddUriAsync(uint instanceId = 0, string objectId = "", uint updateId = 0,
            string enqueuedUri = null, string enqueuedUriMetaData = null, uint addAtIndex = 4294967295,
            CancellationToken cancellationToken = default)
        {
            return InvokeAsync("AddURI", cancellationToken, ("InstanceID", instanceId), ("ObjectID", objectId ?? ""),
                ("UpdateID", updateId), ("AddAtIndex", addAtIndex),
                ("EnqueuedURI", enqueuedUri ?? ""), ("EnqueuedURIMetaData", enqueuedUriMetaData ?? ""));
        }
    }
}