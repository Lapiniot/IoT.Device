using System;

namespace IoT.Device.Lumi
{
    public class ExportSubDeviceAttribute : ExportDeviceAttributeBase
    {
        public ExportSubDeviceAttribute(string modelName, Type implementation) :
            base(0, implementation, modelName)
        {
        }
    }
}