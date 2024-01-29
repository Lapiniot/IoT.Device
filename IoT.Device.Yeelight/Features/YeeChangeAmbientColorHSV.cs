namespace IoT.Device.Yeelight.Features;

public sealed class YeeChangeAmbientColorHSV(YeelightDevice device) : YeeChangeColorHSV(device, "bg_hue", "bg_sat", "bg_set_hsv")
{
}