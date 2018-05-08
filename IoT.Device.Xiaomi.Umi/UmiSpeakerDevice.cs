using System;
using IoT.Device.Upnp;
using IoT.Device.Xiaomi.Umi.Services;
using IoT.Protocol.Soap;
using static System.Threading.LazyThreadSafetyMode;
using static System.UriKind;
using static System.UriPartial;

namespace IoT.Device.Xiaomi.Umi
{
    public class UmiSpeakerDevice : UpnpDevice, IDisposable
    {
        private readonly Lazy<AVTransportService> avtransportLazy;
        private readonly Lazy<ContentDirectoryService> contentDirectoryLazy;
        private readonly Lazy<PlaylistService> playlistLazy;
        private readonly Lazy<SystemPropertiesService> systemPropertiesLazy;
        private readonly Lazy<ConnectionManagerService> connectionManagerLazy;
        private readonly Lazy<RenderingControlService> renderingControlLazy;
        private Lazy<SoapControlEndpoint> endpointLazy;

        public UmiSpeakerDevice(Uri descriptionUri, string usn) : base(descriptionUri, usn)
        {
            DeviceId = usn.Split(new[] { ':' }, 3)[1];

            BaseUri = new Uri(descriptionUri.GetLeftPart(Authority));

            endpointLazy = new Lazy<SoapControlEndpoint>(() => new SoapControlEndpoint(BaseUri), ExecutionAndPublication);

            contentDirectoryLazy = new Lazy<ContentDirectoryService>(() => new ContentDirectoryService(this));
            playlistLazy = new Lazy<PlaylistService>(() => new PlaylistService(this));
            avtransportLazy = new Lazy<AVTransportService>(() => new AVTransportService(this));
            systemPropertiesLazy = new Lazy<SystemPropertiesService>(() => new SystemPropertiesService(this));
            connectionManagerLazy = new Lazy<ConnectionManagerService>(() => new ConnectionManagerService(this));
            renderingControlLazy = new Lazy<RenderingControlService>(() => new RenderingControlService(this));
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