using System;
using IoT.Device.Upnp;
using IoT.Protocol;
using IoT.Protocol.Upnp;

namespace IoT.Device.Xiaomi.Umi
{
    public class UmiSpeakerEnumerator : ConvertingEnumerator<SsdpReply, UmiSpeakerDevice>
    {
        public UmiSpeakerEnumerator() :
            base(new SsdpSearchEnumerator("urn:schemas-upnp-org:device:UmiSystem:1"), new UpnpReplyComparer()) {}

        #region Overrides of ConvertingEnumerator<SsdpReply,UmiSpeakerDevice>

        protected override UmiSpeakerDevice Convert(SsdpReply reply)
        {
            if(reply is null) throw new ArgumentNullException(nameof(reply));
            return reply.StartLine.StartsWith("HTTP", StringComparison.InvariantCulture)
                ? new UmiSpeakerDevice(new Uri(reply.Location), reply.UniqueServiceName)
                : null;
        }

        #endregion
    }
}