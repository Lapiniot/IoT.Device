using System;

namespace IoT.Device.Yeelight.Features
{
    public class YeeAdjustColorTemperature : YeeAdjustPropertyValue
    {
        public static readonly Type Type = typeof(YeeAdjustColorTemperature);

        public YeeAdjustColorTemperature(YeelightDevice device) : base(device, "adjust_ct") { }
    }
}