using IoT.Protocol.Soap;
using IoT.Device.Upnp.Services;
using IoT.Device.Upnp.Umi.Services;
using static System.UriKind;
using static System.UriPartial;
using static System.Net.DecompressionMethods;

namespace IoT.Device.Upnp.Umi;

public sealed class UmiSpeakerDevice : UpnpDevice, IDisposable
{
    private readonly HttpClientHandler handler;
    private readonly HttpClient client;
    private SoapControlEndpoint endpoint;
    private AVTransportService avTransport;
    private ConnectionManagerService connectionManager;
    private ContentDirectoryService contentDirectory;
    private PlaylistService playlist;
    private RenderingControlService renderingControl;
    private SystemPropertiesService systemProperties;

    public UmiSpeakerDevice(Uri descriptionUri, string usn) : base(descriptionUri, usn)
    {
        ArgumentNullException.ThrowIfNull(descriptionUri);

        if (string.IsNullOrEmpty(usn)) throw new ArgumentException("message", nameof(usn));

        DeviceId = usn.Split(new[] { ':' }, 3)[1];

        BaseUri = new(descriptionUri.GetLeftPart(Authority));

        handler = new() { AutomaticDecompression = GZip | Deflate, UseCookies = false, CheckCertificateRevocationList = true };

        client = new(handler, false) { BaseAddress = BaseUri, DefaultRequestHeaders = { { "Accept-Encoding", "gzip" } } };
    }

    public string DeviceId { get; }

    public Uri BaseUri { get; }

    internal SoapControlEndpoint Endpoint => endpoint ??= new(new SoapHttpClient(client));

    public ContentDirectoryService ContentDirectory =>
        contentDirectory ??= new(Endpoint,
                new($"{DeviceId}-MS/upnp.org-ContentDirectory-1/control", Relative));

    public PlaylistService Playlist =>
        playlist ??= new(Endpoint,
                new($"{DeviceId}-MR/xiaomi.com-Playlist-1/control", Relative));

    public AVTransportService AVTransport =>
        avTransport ??= new(Endpoint,
                new($"{DeviceId}-MR/upnp.org-AVTransport-1/control", Relative));

    public SystemPropertiesService SystemProperties =>
        systemProperties ??= new(Endpoint,
                new($"{DeviceId}/xiaomi.com-SystemProperties-1/control", Relative));

    public ConnectionManagerService ConnectionManager =>
        connectionManager ??= new(Endpoint,
                new($"{DeviceId}-MR/upnp.org-ConnectionManager-1/control", Relative));

    public RenderingControlService RenderingControl =>
        renderingControl ??= new(Endpoint,
                new($"{DeviceId}-MR/upnp.org-RenderingControl-1/control", Relative));

    public void Dispose()
    {
        handler.Dispose();
        client.Dispose();
    }
}