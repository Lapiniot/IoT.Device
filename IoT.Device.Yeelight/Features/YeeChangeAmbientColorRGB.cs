namespace IoT.Device.Yeelight.Features;

public class YeeChangeAmbientColorRGB : YeeChangeColorRGB
{
    public YeeChangeAmbientColorRGB(YeelightDevice device) : base(device, "bg_rgb", "bg_set_rgb") { }
}