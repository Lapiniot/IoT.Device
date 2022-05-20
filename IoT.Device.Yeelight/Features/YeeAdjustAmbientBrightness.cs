namespace IoT.Device.Yeelight.Features;

public class YeeAdjustAmbientBrightness : YeeAdjustPropertyValue
{
    public YeeAdjustAmbientBrightness(YeelightDevice device) : base(device, "bg_adjust_bright") { }
}