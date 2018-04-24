using System;

namespace IoT.Device.Lumi
{
    public class SupportedSubDeviceAttribute : SupportedDeviceAttributeBase
    {
        public SupportedSubDeviceAttribute(string modelName, Type implementation) :
            base(0, implementation, modelName)
        {
        }
    }
}