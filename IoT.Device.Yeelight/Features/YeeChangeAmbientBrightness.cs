namespace IoT.Device.Yeelight.Features;

public class YeeChangeAmbientBrightness(YeelightDevice device) : YeeChangeBrightness(device, "bg_bright", "bg_set_bright")
{
}