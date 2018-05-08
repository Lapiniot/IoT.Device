using IoT.Protocol.Soap;
using IoT.Protocol.Upnp;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class RenderingControlService : SoapActionInvoker
    {
        public RenderingControlService(UmiSpeakerDevice parent) : base(parent.Endpoint,
            $"{parent.DeviceId}-MR/upnp.org-RenderingControl-1/control", UpnpServices.RenderingControl)
        {
        }
    }
}