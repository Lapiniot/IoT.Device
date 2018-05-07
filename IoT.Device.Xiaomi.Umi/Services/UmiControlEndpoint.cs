using System;
using IoT.Protocol.Upnp;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public class UmiControlEndpoint : UpnpControlEndpoint
    {
        protected internal UmiControlEndpoint(UmiSpeakerDevice parentDevice, string controlPath, string schema) :
            base(new Uri(parentDevice.BaseUri, controlPath), schema)
        {
        }
    }
}