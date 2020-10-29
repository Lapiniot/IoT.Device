using System;
using System.Policies;
using IoT.Protocol;
using IoT.Protocol.Upnp;

namespace IoT.Device.Upnp
{
    public class UpnpDeviceEnumerator : ConvertingEnumerator<SsdpReply, UpnpDevice>
    {
        public UpnpDeviceEnumerator(string searchTarget, IRepeatPolicy discoveryPolicy) :
            base(new SsdpSearchEnumerator(searchTarget, discoveryPolicy), new UpnpReplyComparer()) {}

        public UpnpDeviceEnumerator(IRepeatPolicy discoveryPolicy) :
            base(new SsdpSearchEnumerator("ssdp:all", discoveryPolicy), new UpnpReplyComparer()) {}

        protected override UpnpDevice Convert(SsdpReply reply)
        {
            if(reply is null) throw new ArgumentNullException(nameof(reply));
            return reply.StartLine.StartsWith("HTTP", StringComparison.InvariantCulture)
                ? new UpnpDevice(new Uri(reply.Location), reply.UniqueServiceName)
                : null;
        }
    }
}