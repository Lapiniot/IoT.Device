using System;

namespace IoT.Device.Yeelight
{
    public sealed class ExportYeelightDeviceAttribute : ExportDeviceAttributeBase
    {
        public ExportYeelightDeviceAttribute(uint deviceType, Type implementation, string modelName) :
            base(deviceType, implementation, modelName)
        { }
    }

}