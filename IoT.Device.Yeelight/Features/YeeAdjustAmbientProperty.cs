namespace IoT.Device.Yeelight.Features;

public class YeeAdjustAmbientProperty : YeeAdjustProperty
{
    public static new readonly Type Type = typeof(YeeAdjustAmbientProperty);

    public YeeAdjustAmbientProperty(YeelightDevice device) : base(device, "bg_set_adjust") { }
}