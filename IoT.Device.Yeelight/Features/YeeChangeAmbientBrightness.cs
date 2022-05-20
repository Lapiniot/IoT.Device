namespace IoT.Device.Yeelight.Features;

public class YeeChangeAmbientBrightness : YeeChangeBrightness
{
    public YeeChangeAmbientBrightness(YeelightDevice device) : base(device, "bg_bright", "bg_set_bright") { }
}