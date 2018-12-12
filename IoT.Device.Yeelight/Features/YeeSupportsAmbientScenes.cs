using System;

namespace IoT.Device.Yeelight.Features
{
    public class YeeSupportsAmbientScenes : YeeSupportsScenes
    {
        public new static readonly Type Type = typeof(YeeSupportsAmbientScenes);

        public YeeSupportsAmbientScenes(YeelightDevice device) :
            base(device, "bg_set_scene")
        { }
    }
}