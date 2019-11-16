using System;

namespace IoT.Device.Yeelight
{
    public abstract class YeelightDeviceFeature
    {
        protected static readonly object[] EmptyArgs = Array.Empty<object>();

        protected YeelightDevice Device;

        protected YeelightDeviceFeature(YeelightDevice device)
        {
            Device = device;
        }

        public abstract string[] SupportedMethods { get; }

        public abstract string[] SupportedProperties { get; }
    }
}