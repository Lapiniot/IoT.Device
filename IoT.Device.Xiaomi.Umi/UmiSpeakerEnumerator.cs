using System;
using System.Policies;
using IoT.Device.Upnp;
using IoT.Protocol;
using IoT.Protocol.Upnp;

namespace IoT.Device.Xiaomi.Umi
{
    public class UmiSpeakerEnumerator : ConvertingEnumerator<SsdpReply, UmiSpeakerDevice>
    {
        public UmiSpeakerEnumerator(IRepeatPolicy discoveryPolicy) :
            base(new SsdpSearchEnumerator("urn:schemas-upnp-org:device:UmiSystem:1", discoveryPolicy), new UpnpReplyComparer()) {}

        #region Overrides of ConvertingEnumerator<SsdpReply,UmiSpeakerDevice>

        protected override UmiSpeakerDevice Convert(SsdpReply thing)
        {
            if(thing is null) throw new ArgumentNullException(nameof(thing));
            return thing.StartLine.StartsWith("HTTP", StringComparison.InvariantCulture)
                ? new UmiSpeakerDevice(new Uri(thing.Location), thing.UniqueServiceName)
                : null;
        }

        #endregion
    }
}