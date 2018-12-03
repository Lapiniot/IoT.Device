using System;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeColorMode : YeelightDeviceCapability
    {
        public YeeChangeColorMode(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();
    }
}