namespace IoT.Device.Yeelight.Features;

public class YeeAdjustAmbientBrightness : YeeAdjustPropertyValue
{
    public static readonly Type Type = typeof(YeeAdjustAmbientBrightness);

    public YeeAdjustAmbientBrightness(YeelightDevice device) : base(device, "bg_adjust_bright") { }
}