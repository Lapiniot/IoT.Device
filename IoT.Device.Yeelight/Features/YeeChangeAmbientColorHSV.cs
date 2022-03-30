namespace IoT.Device.Yeelight.Features;

public sealed class YeeChangeAmbientColorHSV : YeeChangeColorHSV
{
    public static new readonly Type Type = typeof(YeeChangeAmbientColorHSV);

    public YeeChangeAmbientColorHSV(YeelightDevice device) :
        base(device, "bg_hue", "bg_sat", "bg_set_hsv")
    { }
}