using IoT.Protocol.Soap;
using IoT.Protocol.Upnp;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class ContentDirectoryService : SoapActionInvoker
    {
        internal ContentDirectoryService(UmiSpeakerDevice parent) : base(parent.Endpoint, "", UpnpServices.ContentDirectory)
        {
        }
    }
}