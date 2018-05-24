using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Soap;
using IoT.Protocol.Upnp;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class ContentDirectoryService : SoapActionInvoker
    {
        internal ContentDirectoryService(UmiSpeakerDevice parent) : base(parent.Endpoint,
            $"{parent.DeviceId}-MS/upnp.org-ContentDirectory-1/control", UpnpServices.ContentDirectory)
        {
        }

        public async Task<string> BrowseAsync(string parent, string filter = null, uint index = 0, uint count = 50,
            string sortCriteria = null, CancellationToken cancellationToken = default)
        {
            return (await InvokeAsync("Browse", cancellationToken,
                ("ObjectID", parent), ("BrowseFlag", "BrowseDirectChildren"),
                ("Filter", filter ?? "*"), ("StartingIndex", index),
                ("RequestedCount", count), ("SortCriteria", sortCriteria ?? "")).ConfigureAwait(false))["Result"];
        }
    }
}