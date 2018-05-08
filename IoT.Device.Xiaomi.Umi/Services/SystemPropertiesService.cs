﻿using IoT.Protocol.Soap;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class SystemPropertiesService : SoapActionInvoker
    {
        internal SystemPropertiesService(UmiSpeakerDevice parent) : base(parent.Endpoint, "", "")
        {
        }
    }
}