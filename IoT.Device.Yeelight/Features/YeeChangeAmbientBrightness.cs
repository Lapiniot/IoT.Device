using System;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeAmbientBrightness : YeeChangeBrightness
    {
        public new static readonly Type Type = typeof(YeeChangeAmbientBrightness);

        public YeeChangeAmbientBrightness(YeelightDevice device) : base(device, "bg_bright", "bg_set_bright") { }
    }
}