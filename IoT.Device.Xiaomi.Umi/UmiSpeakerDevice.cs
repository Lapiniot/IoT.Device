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
        private Lazy<SoapControlEndpoint> endpointLazy;

        public UmiSpeakerDevice(Uri descriptionUri, string usn) : base(descriptionUri, usn)
        {
            BaseUri = new Uri(new Uri(descriptionUri.GetLeftPart(Authority), Absolute), descriptionUri.Segments[0]);

            endpointLazy = new Lazy<SoapControlEndpoint>(() => new SoapControlEndpoint(BaseUri), ExecutionAndPublication);

            contentDirectoryLazy = new Lazy<ContentDirectoryService>(() => new ContentDirectoryService(this));
            playlistLazy = new Lazy<PlaylistService>(() => new PlaylistService(this));
            avtransportLazy = new Lazy<AVTransportService>(() => new AVTransportService(this));
            systemPropertiesLazy = new Lazy<SystemPropertiesService>(() => new SystemPropertiesService(this));
        }

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

        #region Implementation of IDisposable

        public void Dispose()
        {
            if(endpointLazy != null && endpointLazy.IsValueCreated)
            {
                endpointLazy.Value.Dispose();
                endpointLazy = null;
            }
        }

        #endregion
    }
}