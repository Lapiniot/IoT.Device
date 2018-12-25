using System;
using IoT.Device.Upnp;
using IoT.Protocol;
using IoT.Protocol.Upnp;

namespace IoT.Device.Xiaomi.Umi
{
    public class UmiSpeakerEnumerator : ConvertingEnumerator<SsdpReply, UmiSpeakerDevice>
    {
        public UmiSpeakerEnumerator() :
            base(new SsdpEnumerator("urn:schemas-upnp-org:device:UmiSystem:1"), new UpnpReplyComparer()) {}

        #region Overrides of ConvertingEnumerator<SsdpReply,UmiSpeakerDevice>

        protected override UmiSpeakerDevice Convert(SsdpReply thing)
        {
            return new UmiSpeakerDevice(new Uri(thing.Location), thing.UniqueServiceName);
        }

        #endregion
    }
}