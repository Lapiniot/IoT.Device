namespace IoT.Device.Yeelight.Features;

public class YeeChangeAmbientColorRGB : YeeChangeColorRGB
{
    public static new readonly Type Type = typeof(YeeChangeAmbientColorRGB);

    public YeeChangeAmbientColorRGB(YeelightDevice device) : base(device, "bg_rgb", "bg_set_rgb") { }
}