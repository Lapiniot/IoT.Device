using System;

namespace IoT.Device.Yeelight.Features
{
    public class YeeAdjustAmbientProperty : YeeAdjustProperty
    {
        public new static readonly Type Type = typeof(YeeAdjustAmbientProperty);

        public YeeAdjustAmbientProperty(YeelightDevice device) : base(device, "bg_set_adjust") { }
    }
}