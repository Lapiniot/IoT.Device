using System.Policies;
using IoT.Protocol;

namespace IoT.Device.Upnp;

public class UpnpDeviceEnumerator : ConvertingEnumerator<SsdpReply, UpnpDevice>
{
    public UpnpDeviceEnumerator(string searchTarget, IRepeatPolicy discoveryPolicy) :
        base(new SsdpSearchEnumerator(searchTarget, discoveryPolicy), new UpnpReplyComparer())
    { }

    public UpnpDeviceEnumerator(IRepeatPolicy discoveryPolicy) :
        base(new SsdpSearchEnumerator("ssdp:all", discoveryPolicy), new UpnpReplyComparer())
    { }

    protected override UpnpDevice Convert([NotNull] SsdpReply thing) =>
        thing.StartLine.StartsWith("HTTP", StringComparison.InvariantCulture)
            ? new UpnpDevice(new(thing.Location), thing.UniqueServiceName)
            : null;
}