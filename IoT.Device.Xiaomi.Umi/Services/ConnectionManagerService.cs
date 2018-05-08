using IoT.Protocol.Soap;
using IoT.Protocol.Upnp;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public class ConnectionManagerService : SoapActionInvoker
    {
        public ConnectionManagerService(UmiSpeakerDevice parent) : base(parent.Endpoint,
            $"{parent.DeviceId}-MR/upnp.org-ConnectionManager-1/control", UpnpServices.ConnectionManager)
        {
        }
    }
}