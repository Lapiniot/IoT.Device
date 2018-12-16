using System;

namespace IoT.Device.Lumi
{
    public class ExportSubDeviceAttribute : ExportDeviceAttributeBase
    {
        public ExportSubDeviceAttribute(string model, Type implementation) :
            base(model, implementation) {}
    }
}