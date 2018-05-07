using static IoT.Protocol.Upnp.UpnpServices;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class AVTransportEndpoint : UmiControlEndpoint
    {
        internal AVTransportEndpoint(UmiSpeakerDevice parentDevice) : base(parentDevice, "", AVTransport)
        {
        }
    }
}