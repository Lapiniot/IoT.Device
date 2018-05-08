using IoT.Protocol.Soap;
using IoT.Protocol.Upnp;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class AVTransportService : SoapActionInvoker
    {
        internal AVTransportService(UmiSpeakerDevice parent) : base(parent.Endpoint, "", UpnpServices.AVTransport)
        {
        }
    }
}