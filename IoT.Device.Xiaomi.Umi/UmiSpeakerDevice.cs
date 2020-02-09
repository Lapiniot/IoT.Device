using System;
using IoT.Device.Upnp;
using IoT.Device.Xiaomi.Umi.Services;
using IoT.Protocol.Soap;
using IoT.Protocol.Upnp.Services;
using static System.Threading.LazyThreadSafetyMode;
using static System.UriKind;
using static System.UriPartial;

namespace IoT.Device.Xiaomi.Umi
{
    public class UmiSpeakerDevice : UpnpDevice
    {
        private readonly Lazy<AVTransportService> avTransportLazy;
        private readonly Lazy<ConnectionManagerService> connectionManagerLazy;
        private readonly Lazy<ContentDirectoryService> contentDirectoryLazy;
        private readonly Lazy<SoapControlEndpoint> endpointLazy;
        private readonly Lazy<PlaylistService> playlistLazy;
        private readonly Lazy<RenderingControlService> renderingControlLazy;
        private readonly Lazy<SystemPropertiesService> systemPropertiesLazy;

        public UmiSpeakerDevice(Uri descriptionUri, string usn) : base(descriptionUri, usn)
        {
            if(descriptionUri is null) throw new ArgumentNullException(nameof(descriptionUri));

            if(string.IsNullOrEmpty(usn)) throw new ArgumentException("message", nameof(usn));

            DeviceId = usn.Split(new[] {':'}, 3)[1];

            BaseUri = new Uri(descriptionUri.GetLeftPart(Authority));

            endpointLazy = new Lazy<SoapControlEndpoint>(() => new SoapControlEndpoint(BaseUri), ExecutionAndPublication);

            contentDirectoryLazy = new Lazy<ContentDirectoryService>(() =>
                new ContentDirectoryService(Endpoint,
                    new Uri($"{DeviceId}-MS/upnp.org-ContentDirectory-1/control", Relative)));
            playlistLazy = new Lazy<PlaylistService>(() =>
                new PlaylistService(Endpoint,
                    new Uri($"{DeviceId}-MR/xiaomi.com-Playlist-1/control", Relative)));
            avTransportLazy = new Lazy<AVTransportService>(() =>
                new AVTransportService(Endpoint,
                    new Uri($"{DeviceId}-MR/upnp.org-AVTransport-1/control", Relative)));
            systemPropertiesLazy = new Lazy<SystemPropertiesService>(() =>
                new SystemPropertiesService(Endpoint,
                    new Uri($"{DeviceId}/xiaomi.com-SystemProperties-1/control", Relative)));
            connectionManagerLazy = new Lazy<ConnectionManagerService>(() =>
                new ConnectionManagerService(Endpoint,
                    new Uri($"{DeviceId}-MR/upnp.org-ConnectionManager-1/control", Relative)));
            renderingControlLazy = new Lazy<RenderingControlService>(() =>
                new RenderingControlService(Endpoint,
                    new Uri($"{DeviceId}-MR/upnp.org-RenderingControl-1/control", Relative)));
        }

        public string DeviceId { get; }

        public Uri BaseUri { get; }

        internal SoapControlEndpoint Endpoint => endpointLazy.Value;

        public ContentDirectoryService ContentDirectory => contentDirectoryLazy.Value;

        public PlaylistService Playlist => playlistLazy.Value;

        public AVTransportService AVTransport => avTransportLazy.Value;

        public SystemPropertiesService SystemProperties => systemPropertiesLazy.Value;

        public ConnectionManagerService ConnectionManager => connectionManagerLazy.Value;

        public RenderingControlService RenderingControl => renderingControlLazy.Value;
    }
}