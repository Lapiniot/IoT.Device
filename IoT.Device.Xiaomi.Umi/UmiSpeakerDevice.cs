using IoT.Device.Upnp;
using IoT.Device.Xiaomi.Umi.Services;
using IoT.Protocol.Soap;
using IoT.Protocol.Upnp.Services;
using static System.UriKind;
using static System.UriPartial;
using static System.Net.DecompressionMethods;

namespace IoT.Device.Xiaomi.Umi;

public sealed class UmiSpeakerDevice : UpnpDevice, IDisposable
{
    private HttpClientHandler handler;
    private HttpClient client;
    private SoapControlEndpoint endpoint;
    private AVTransportService avTransport;
    private ConnectionManagerService connectionManager;
    private ContentDirectoryService contentDirectory;
    private PlaylistService playlist;
    private RenderingControlService renderingControl;
    private SystemPropertiesService systemProperties;

    public UmiSpeakerDevice(Uri descriptionUri, string usn) : base(descriptionUri, usn)
    {
        if(descriptionUri is null) throw new ArgumentNullException(nameof(descriptionUri));

        if(string.IsNullOrEmpty(usn)) throw new ArgumentException("message", nameof(usn));

        DeviceId = usn.Split(new[] { ':' }, 3)[1];

        BaseUri = new Uri(descriptionUri.GetLeftPart(Authority));

        handler = new() { AutomaticDecompression = GZip | Deflate, UseCookies = false, CheckCertificateRevocationList = true };

        client = new(handler, false) { BaseAddress = BaseUri, DefaultRequestHeaders = { { "Accept-Encoding", "gzip" } } };
    }

    public string DeviceId { get; }

    public Uri BaseUri { get; }

    internal SoapControlEndpoint Endpoint => endpoint ??= new(client);

    public ContentDirectoryService ContentDirectory =>
        contentDirectory ??= new(Endpoint,
                new Uri($"{DeviceId}-MS/upnp.org-ContentDirectory-1/control", Relative));

    public PlaylistService Playlist =>
        playlist ??= new(Endpoint,
                new Uri($"{DeviceId}-MR/xiaomi.com-Playlist-1/control", Relative));

    public AVTransportService AVTransport =>
        avTransport ??= new(Endpoint,
                new Uri($"{DeviceId}-MR/upnp.org-AVTransport-1/control", Relative));

    public SystemPropertiesService SystemProperties =>
        systemProperties ??= new(Endpoint,
                new Uri($"{DeviceId}/xiaomi.com-SystemProperties-1/control", Relative));

    public ConnectionManagerService ConnectionManager =>
        connectionManager ??= new(Endpoint,
                new Uri($"{DeviceId}-MR/upnp.org-ConnectionManager-1/control", Relative));

    public RenderingControlService RenderingControl =>
        renderingControl ??= new(Endpoint,
                new Uri($"{DeviceId}-MR/upnp.org-RenderingControl-1/control", Relative));

    public void Dispose()
    {
        handler.Dispose();
        client.Dispose();
    }
}