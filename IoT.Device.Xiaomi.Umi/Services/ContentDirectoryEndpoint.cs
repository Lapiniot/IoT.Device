using static IoT.Protocol.Upnp.UpnpServices;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class ContentDirectoryEndpoint : UmiControlEndpoint
    {
        internal ContentDirectoryEndpoint(UmiSpeakerDevice parentDevice) : base(parentDevice, "", ContentDirectory)
        {
        }
    }
}