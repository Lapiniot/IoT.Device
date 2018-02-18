using System;

namespace IoT.Device.Upnp
{
    public class UpnpDevice
    {
        public UpnpDevice(Uri location)
        {
            Location = location;
        }

        public Uri Location { get; }
    }
}