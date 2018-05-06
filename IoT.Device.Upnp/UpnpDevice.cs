using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using IoT.Protocol.Upnp;

namespace IoT.Device.Upnp
{
    public class UpnpDevice
    {
        public string Usn { get; }
        public UpnpDevice(Uri descriptionUri, string usn)
        {
            DescriptionUri = descriptionUri ?? throw new ArgumentNullException(nameof(descriptionUri));

            if(string.IsNullOrWhiteSpace(usn)) throw new ArgumentException("valid USN must be provided", nameof(usn));
            
            Usn = usn;
        }

        public Uri DescriptionUri { get; }

        public Task<UpnpDeviceDescription> GetDescriptionAsync(CancellationToken cancellationToken = default)
        {
            return UpnpDeviceDescription.LoadAsync(DescriptionUri, cancellationToken);

        }
    }
}