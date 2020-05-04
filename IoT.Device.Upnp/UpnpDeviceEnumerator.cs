using System;
using IoT.Protocol;
using IoT.Protocol.Upnp;

namespace IoT.Device.Upnp
{
    public class UpnpDeviceEnumerator : ConvertingEnumerator<SsdpReply, UpnpDevice>
    {
        public UpnpDeviceEnumerator(TimeSpan pollInterval, string searchTarget = "ssdp:all") :
            base(new SsdpSearchEnumerator(pollInterval, searchTarget), new UpnpReplyComparer()) {}

        protected override UpnpDevice Convert(SsdpReply reply)
        {
            if(reply is null) throw new ArgumentNullException(nameof(reply));
            return reply.StartLine.StartsWith("HTTP", StringComparison.InvariantCulture)
                ? new UpnpDevice(new Uri(reply.Location), reply.UniqueServiceName)
                : null;
        }
    }
}