namespace IoT.Device.Yeelight.Features;

public class YeeAdjustColor : YeeAdjustPropertyValue
{
    public static readonly Type Type = typeof(YeeAdjustColor);

    public YeeAdjustColor(YeelightDevice device) : base(device, "adjust_color") { }
}