using IoT.Protocol;
using OOs.Policies;

namespace IoT.Device.Upnp.Umi;

public class UmiSpeakerEnumerator(IRepeatPolicy discoveryPolicy) : ConvertingEnumerator<SsdpReply, UmiSpeakerDevice>(new SsdpSearchEnumerator("urn:schemas-upnp-org:device:UmiSystem:1", discoveryPolicy), new UpnpReplyComparer())
{
    #region Overrides of ConvertingEnumerator<SsdpReply,UmiSpeakerDevice>

    protected override UmiSpeakerDevice Convert([NotNull] SsdpReply thing) =>
        thing.StartLine.StartsWith("HTTP", StringComparison.InvariantCulture)
            ? new UmiSpeakerDevice(new(thing.Location), thing.UniqueServiceName)
            : null;

    #endregion
}