namespace IoT.Device.Yeelight
{
    public abstract class YeelightDeviceFeature
    {
        protected YeelightDevice Device;

        protected YeelightDeviceFeature(YeelightDevice device)
        {
            Device = device;
        }

        public abstract string[] SupportedMethods { get; }

        public abstract string[] SupportedProperties { get; }
    }
}