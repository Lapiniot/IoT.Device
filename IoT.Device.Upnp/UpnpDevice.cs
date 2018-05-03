using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using IoT.Protocol.Upnp;

namespace IoT.Device.Upnp
{
    public class UpnpDevice
    {
        public UpnpDevice(Uri location)
        {
            Location = location;
        }

        public Uri Location { get; }

        public Task<UpnpDeviceDescription> GetDescriptionAsync(CancellationToken cancellationToken = default)
        {
            return UpnpDeviceDescription.LoadAsync(Location, cancellationToken);
            
        }
    }
}