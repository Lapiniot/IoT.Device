using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Soap;
using IoT.Protocol.Upnp.Services;
using static System.UriKind;

namespace IoT.Device.Xiaomi.Umi.Services
{
    [ServiceSchema(ServiceSchema)]
    public sealed class PlaylistService : SoapActionInvoker
    {
        public const string ServiceSchema = "urn:xiaomi-com:service:Playlist:1";
        public PlaylistService(SoapControlEndpoint endpoint, Uri controlUri) :
            base(endpoint, controlUri, ServiceSchema)
        {
        }

        public PlaylistService(SoapControlEndpoint endpoint) :
            base(endpoint, ServiceSchema)
        {
        }

        public Task<IDictionary<string, string>> CreateAsync(uint instanceId = 0,
            string title = "", string enqueuedUri = null, string enqueuedUriMetaData = null,
            CancellationToken cancellationToken = default)
        {
            return InvokeAsync("Create", cancellationToken, ("InstanceID", instanceId), ("Title", title ?? ""),
                ("EnqueuedURI", enqueuedUri ?? ""), ("EnqueuedURIMetaData", enqueuedUriMetaData ?? ""));
        }

        public Task<IDictionary<string, string>> DeleteAsync(uint instanceId = 0, string updateId = "0",
            int[] indeces = null, CancellationToken cancellationToken = default)
        {
            if(indeces == null) throw new ArgumentNullException(nameof(indeces));
            if(indeces.Length == 0) throw new ArgumentException("Must not be empty!", nameof(indeces));
            return InvokeAsync("ReorderPlaylists", cancellationToken,
                ("InstanceID", instanceId), ("ObjectID", "PL:"), ("UpdateID", updateId),
                ("Playlists", string.Join(',', indeces)), ("NewPositionList", "".PadRight(indeces.Length - 1, ',')));
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