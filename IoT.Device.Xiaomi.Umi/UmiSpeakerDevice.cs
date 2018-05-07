using System;
using IoT.Device.Upnp;
using IoT.Device.Xiaomi.Umi.Services;
using static System.UriKind;
using static System.UriPartial;

namespace IoT.Device.Xiaomi.Umi
{
    public class UmiSpeakerDevice : UpnpDevice
    {
        private readonly Lazy<ContentDirectoryEndpoint> contentEndpointLazy;

        public UmiSpeakerDevice(Uri descriptionUri, string usn) : base(descriptionUri, usn)
        {
            BaseUri = new Uri(new Uri(descriptionUri.GetLeftPart(Authority), Absolute), descriptionUri.Segments[0]);
            contentEndpointLazy = new Lazy<ContentDirectoryEndpoint>(() => new ContentDirectoryEndpoint(this));
        }

        public Uri BaseUri { get; }

        public ContentDirectoryEndpoint ContentEndpointLazy
        {
            get { return contentEndpointLazy.Value; }
        }


    }
}