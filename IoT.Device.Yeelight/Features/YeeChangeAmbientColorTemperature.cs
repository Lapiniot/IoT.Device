namespace IoT.Device.Yeelight.Features;

public class YeeChangeAmbientColorTemperature(YeelightDevice device) : YeeChangeColorTemperature(device, "bg_ct", "bg_set_ct_abx") { }