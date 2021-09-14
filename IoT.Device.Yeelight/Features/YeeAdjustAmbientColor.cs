namespace IoT.Device.Yeelight.Features;

public class YeeAdjustAmbientColor : YeeAdjustPropertyValue
{
    public static readonly Type Type = typeof(YeeAdjustAmbientColor);

    public YeeAdjustAmbientColor(YeelightDevice device) : base(device, "bg_adjust_color") { }
}