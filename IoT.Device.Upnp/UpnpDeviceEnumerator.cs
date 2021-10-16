using System.Diagnostics.CodeAnalysis;
using System.Policies;
using IoT.Protocol;
using IoT.Protocol.Upnp;

namespace IoT.Device.Upnp;

public class UpnpDeviceEnumerator : ConvertingEnumerator<SsdpReply, UpnpDevice>
{
    public UpnpDeviceEnumerator(string searchTarget, IRepeatPolicy discoveryPolicy) :
        base(new SsdpSearchEnumerator(searchTarget, discoveryPolicy), new UpnpReplyComparer())
    { }

    public UpnpDeviceEnumerator(IRepeatPolicy discoveryPolicy) :
        base(new SsdpSearchEnumerator("ssdp:all", discoveryPolicy), new UpnpReplyComparer())
    { }

    protected override UpnpDevice Convert([NotNull] SsdpReply thing)
    {
        return thing.StartLine.StartsWith("HTTP", StringComparison.InvariantCulture)
            ? new UpnpDevice(new Uri(thing.Location), thing.UniqueServiceName)
            : null;
    }
}