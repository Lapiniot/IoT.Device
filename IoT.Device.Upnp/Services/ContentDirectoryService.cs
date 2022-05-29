using System.Runtime.CompilerServices;

namespace IoT.Device.Upnp.Services;

public enum BrowseMode
{
    BrowseDirectChildren,
    BrowseMetadata
}

[CLSCompliant(false)]
[ExportService(ContentDirectory)]
public sealed class ContentDirectoryService : SoapActionInvoker, IUpnpService, IUpnpServiceFactory<ContentDirectoryService>
{
    public static string ServiceSchema => ContentDirectory;

    public ContentDirectoryService(SoapControlEndpoint endpoint, Uri controlUri) :
        base(endpoint, controlUri, ContentDirectory)
    { }

    public Task<IReadOnlyDictionary<string, string>> BrowseAsync(string parent, string filter = null,
        BrowseMode mode = default, string sortCriteria = null,
        uint index = 0, uint count = 50, CancellationToken cancellationToken = default) =>
        InvokeAsync("Browse", new Dictionary<string, string>
            {
                { "ObjectID", parent },
                { "BrowseFlag", mode.ToString() },
                { "Filter", filter ?? "*" },
                { "StartingIndex", index.ToString(InvariantCulture) },
                { "RequestedCount", count.ToString(InvariantCulture) },
                { "SortCriteria", sortCriteria ?? "" } },
            cancellationToken);

    public async IAsyncEnumerable<(string Content, int matches, int total)> BrowseChildrenAsync(string parent,
        string filter = null, string sortCriteria = null, uint pageSize = 50,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        uint total;
        var fetched = 0u;
        do
        {
            var data = await BrowseAsync(parent, filter, BrowseMode.BrowseDirectChildren, sortCriteria, fetched, pageSize, cancellationToken).ConfigureAwait(false);
            total = uint.Parse(data["TotalMatches"], InvariantCulture);
            var count = uint.Parse(data["NumberReturned"], InvariantCulture);
            fetched += count;
            yield return (data["Result"], (int)count, (int)total);
        }
        while (fetched < total);
    }

    public Task<IReadOnlyDictionary<string, string>> SearchAsync(string container, string query, string filter = null,
        string sortCriteria = null, uint index = 0, uint count = 50, CancellationToken cancellationToken = default) =>
        InvokeAsync("Search", new Dictionary<string, string>
            {
                { "ContainerID", container },
                { "SearchCriteria", query },
                { "Filter", filter ?? "*" },
                { "StartingIndex", index.ToString(InvariantCulture) },
                { "RequestedCount", count.ToString(InvariantCulture) },
                { "SortCriteria", sortCriteria ?? "" } },
            cancellationToken);

    public static ContentDirectoryService Create(SoapControlEndpoint endpoint, Uri controlUri) => new(endpoint, controlUri);
}