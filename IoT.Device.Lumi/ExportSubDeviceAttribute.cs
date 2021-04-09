using System;

namespace IoT.Device.Lumi
{
    public sealed class ExportSubDeviceAttribute : ExportDeviceAttributeBase
    {
        public ExportSubDeviceAttribute(string model, Type implementationType) :
            base(model, implementationType) {}
    }
}