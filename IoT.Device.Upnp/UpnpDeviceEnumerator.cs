using System;
using IoT.Protocol;
using IoT.Protocol.Upnp;

namespace IoT.Device.Upnp
{
    public class UpnpDeviceEnumerator : ConvertingEnumerator<SsdpReply, UpnpDevice>
    {
        public UpnpDeviceEnumerator(TimeSpan pollInterval, string searchTarget = "ssdp:all") :
            base(new SsdpEnumerator(pollInterval, searchTarget), new UpnpReplyComparer()) {}

        protected override UpnpDevice Convert(SsdpReply reply)
        {
            if(reply is null) throw new ArgumentNullException(nameof(reply));

            return new UpnpDevice(new Uri(reply.Location), reply.UniqueServiceName);
        }
    }
}