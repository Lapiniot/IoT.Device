using System;
using IoT.Protocol;
using IoT.Protocol.Upnp;

namespace IoT.Device.Upnp
{
    public class UpnpDeviceEnumerator : ConvertingEnumerator<SsdpReply, UpnpDevice>
    {
        public UpnpDeviceEnumerator(string searchTarget = "ssdp:all") :
            base(new SsdpEnumerator(searchTarget: searchTarget)) {}

        public override UpnpDevice Convert(SsdpReply reply)
        {
            return new UpnpDevice(new Uri(reply.Location), reply.UniqueServiceName);
        }
    }
}