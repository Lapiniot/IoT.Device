using System;

namespace IoT.Device.Yeelight
{
    public sealed class ExportYeelightDeviceAttribute : ExportDeviceAttributeBase
    {
        public ExportYeelightDeviceAttribute(string model, Type implementationType) :
            base(model, implementationType) {}
    }
}