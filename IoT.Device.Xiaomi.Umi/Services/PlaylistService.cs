using System;
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

        public Task<IDictionary<string, string>> CreateAsync(uint instanceID = 0,
            string title = "", string enqueuedURI = null, string enqueuedURIMetaData = null,
            CancellationToken cancellationToken = default)
        {
            return InvokeAsync("Create", cancellationToken, ("InstanceID", instanceID), ("Title", title ?? ""),
                ("EnqueuedURI", enqueuedURI ?? ""), ("EnqueuedURIMetaData", enqueuedURIMetaData ?? ""));
        }

        public Task<IDictionary<string, string>> AddUriAsync(uint instanceID = 0, string objectID = "", uint updateID = 0,
            string enqueuedURI = null, string enqueuedURIMetaData = null, uint addAtIndex = 4294967295,
            CancellationToken cancellationToken = default)
        {
            return InvokeAsync("AddURI", cancellationToken, ("InstanceID", instanceID), ("ObjectID", objectID ?? ""),
                ("UpdateID", updateID), ("AddAtIndex", addAtIndex),
                ("EnqueuedURI", enqueuedURI ?? ""), ("EnqueuedURIMetaData", enqueuedURIMetaData ?? ""));
        }
    }
}