namespace IoT.Device.Yeelight.Features;

public class YeeProvideAmbientColorMode : YeeProvideColorMode
{
    public static new readonly Type Type = typeof(YeeProvideAmbientColorMode);

    public YeeProvideAmbientColorMode(YeelightDevice device) : base(device, "bg_lmode") { }
}