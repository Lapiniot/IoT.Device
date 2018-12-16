using System;

namespace IoT.Device.Yeelight
{
    public sealed class ExportYeelightDeviceAttribute : ExportDeviceAttributeBase
    {
        public ExportYeelightDeviceAttribute(string model, Type implementation) :
            base(model, implementation) {}
    }
}