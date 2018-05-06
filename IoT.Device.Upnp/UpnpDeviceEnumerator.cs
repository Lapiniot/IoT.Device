using System;
using System.Collections.Generic;
using IoT.Protocol;
using IoT.Protocol.Upnp;

namespace IoT.Device.Upnp
{
    public class UpnpDeviceEnumerator : ConvertingEnumerator<IDictionary<string, string>, UpnpDevice>
    {
        public UpnpDeviceEnumerator(string searchTarget = "ssdp:all") : base(new UpnpEnumerator(searchTarget))
        {
        }

        public override UpnpDevice Convert(IDictionary<string, string> thing)
        {
            var usn = thing["USN"];

            return new UpnpDevice(new Uri(thing["LOCATION"]), usn);
        }
    }
}