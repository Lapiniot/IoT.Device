namespace IoT.Device.Yeelight.Features;

public class YeeAdjustBrightness : YeeAdjustPropertyValue
{
    public static readonly Type Type = typeof(YeeAdjustBrightness);

    public YeeAdjustBrightness(YeelightDevice device) : base(device, "adjust_bright") { }
}