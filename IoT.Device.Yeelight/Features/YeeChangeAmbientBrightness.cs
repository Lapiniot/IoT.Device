namespace IoT.Device.Yeelight.Features;

public class YeeChangeAmbientBrightness : YeeChangeBrightness
{
    public static new readonly Type Type = typeof(YeeChangeAmbientBrightness);

    public YeeChangeAmbientBrightness(YeelightDevice device) : base(device, "bg_bright", "bg_set_bright") { }
}