namespace IoT.Device.Yeelight.Features;

public class YeeSupportsAmbientScenes : YeeSupportsScenes
{
    public YeeSupportsAmbientScenes(YeelightDevice device) :
        base(device, "bg_set_scene")
    { }
}