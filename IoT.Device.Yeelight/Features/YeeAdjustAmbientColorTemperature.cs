using System;

namespace IoT.Device.Yeelight.Features
{
    public class YeeAdjustAmbientColorTemperature : YeeAdjustPropertyValue
    {
        public static readonly Type Type = typeof(YeeAdjustAmbientColorTemperature);

        public YeeAdjustAmbientColorTemperature(YeelightDevice device) : base(device, "bg_adjust_ct") { }
    }
}