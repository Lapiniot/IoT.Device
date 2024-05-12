namespace IoT.Device.Yeelight.Features;

public sealed class ChangeAmbientColorHSV(YeelightDevice device) : ChangeColorHSV(device, "bg_hue", "bg_sat", "bg_set_hsv") { }