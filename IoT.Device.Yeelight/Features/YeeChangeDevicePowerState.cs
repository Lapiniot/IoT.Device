using System;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeDevicePowerState : YeeChangePowerState
    {
        public new static readonly Type Type = typeof(YeeChangeDevicePowerState);

        public YeeChangeDevicePowerState(YeelightDevice device) :
            base(device, "main_power", "set_main_power", "dev_toggle")
        { }
    }
}