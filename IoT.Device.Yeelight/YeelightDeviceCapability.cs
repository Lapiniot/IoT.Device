namespace IoT.Device.Yeelight
{
    public abstract class YeelightDeviceCapability
    {
        protected YeelightDevice Device;

        protected YeelightDeviceCapability(YeelightDevice device)
        {
            Device = device;
        }

        public abstract string[] SupportedMethods { get; }

        public abstract string[] SupportedProperties { get; }
    }
}