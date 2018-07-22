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
        private readonly Lazy<AVTransportService> avtransportLazy;
        private readonly Lazy<ConnectionManagerService> connectionManagerLazy;
        private readonly Lazy<ContentDirectoryService> contentDirectoryLazy;
        private readonly Lazy<SoapControlEndpoint> endpointLazy;
        private readonly Lazy<PlaylistService> playlistLazy;
        private readonly Lazy<RenderingControlService> renderingControlLazy;
        private readonly Lazy<SystemPropertiesService> systemPropertiesLazy;

        public UmiSpeakerDevice(Uri descriptionUri, string usn) : base(descriptionUri, usn)
        {
            DeviceId = usn.Split(new[] { ':' }, 3)[1];

            BaseUri = new Uri(descriptionUri.GetLeftPart(Authority));

            endpointLazy = new Lazy<SoapControlEndpoint>(() => new SoapControlEndpoint(BaseUri), ExecutionAndPublication);

            contentDirectoryLazy = new Lazy<ContentDirectoryService>(() =>
                new ContentDirectoryService(Endpoint, 
                    new Uri($"{DeviceId}-MS/upnp.org-ContentDirectory-1/control", Relative)));
            playlistLazy = new Lazy<PlaylistService>(() =>
                new PlaylistService(Endpoint, 
                    new Uri($"{DeviceId}-MR/xiaomi.com-Playlist-1/control", Relative)));
            avtransportLazy = new Lazy<AVTransportService>(() =>
                new AVTransportService(Endpoint, 
                    new Uri($"{DeviceId}-MR/upnp.org-AVTransport-1/control", UriKind.Relative)));
            systemPropertiesLazy = new Lazy<SystemPropertiesService>(() =>
                new SystemPropertiesService(Endpoint, 
                    new Uri($"{DeviceId}/xiaomi.com-SystemProperties-1/control", UriKind.Relative)));
            connectionManagerLazy = new Lazy<ConnectionManagerService>(() => 
                new ConnectionManagerService(Endpoint, 
                    new Uri($"{DeviceId}-MR/upnp.org-ConnectionManager-1/control", UriKind.Relative)));
            renderingControlLazy = new Lazy<RenderingControlService>(() => 
                new RenderingControlService(Endpoint, 
                    new Uri($"{DeviceId}-MR/upnp.org-RenderingControl-1/control", UriKind.Relative)));
        }

        public string DeviceId { get; }

        public Uri BaseUri { get; }

        internal SoapControlEndpoint Endpoint
        {
            get { return endpointLazy.Value; }
        }

        public ContentDirectoryService ContentDirectory
        {
            get { return contentDirectoryLazy.Value; }
        }

        public PlaylistService Playlist
        {
            get { return playlistLazy.Value; }
        }

        public AVTransportService AVTransport
        {
            get { return avtransportLazy.Value; }
        }

        public SystemPropertiesService SystemProperties
        {
            get { return systemPropertiesLazy.Value; }
        }

        public ConnectionManagerService ConnectionManager
        {
            get { return connectionManagerLazy.Value; }
        }

        public RenderingControlService RenderingControl
        {
            get { return renderingControlLazy.Value; }
        }

        protected override void OnConnect()
        {
            base.OnConnect();
            Endpoint.Connect();
        }

        protected override void OnClose()
        {
            base.OnClose();
            Endpoint.Close();
        }
    }
}