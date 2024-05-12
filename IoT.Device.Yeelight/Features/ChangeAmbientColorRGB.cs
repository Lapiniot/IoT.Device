namespace IoT.Device.Yeelight.Features;

public class ChangeAmbientColorRGB(YeelightDevice device) : ChangeColorRGB(device, "bg_rgb", "bg_set_rgb") { }