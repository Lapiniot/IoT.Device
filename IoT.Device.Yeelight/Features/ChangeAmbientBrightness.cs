namespace IoT.Device.Yeelight.Features;

public class ChangeAmbientBrightness(YeelightDevice device) : ChangeBrightness(device, "bg_bright", "bg_set_bright") { }