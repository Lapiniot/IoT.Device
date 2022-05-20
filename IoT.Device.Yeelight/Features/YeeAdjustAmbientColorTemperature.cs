namespace IoT.Device.Yeelight.Features;

public class YeeAdjustAmbientColorTemperature : YeeAdjustPropertyValue
{
    public YeeAdjustAmbientColorTemperature(YeelightDevice device) : base(device, "bg_adjust_ct") { }
}