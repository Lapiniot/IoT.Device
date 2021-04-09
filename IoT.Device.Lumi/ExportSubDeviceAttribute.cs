using System;

namespace IoT.Device.Lumi
{
    public sealed class ExportSubDeviceAttribute : ExportDeviceAttributeBase
    {
        public ExportSubDeviceAttribute(string model, Type implementation) :
            base(model, implementation) {}
    }
}