using System;

namespace IoT.Device.Yeelight.Features
{
    public class YeeSupportsFlowMode : YeelightDeviceCapability
    {
        public YeeSupportsFlowMode(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();
    }
}