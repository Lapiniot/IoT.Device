namespace IoT.Device.Yeelight.Features;

public class ChangeAmbientColorTemperature(YeelightDevice device) : ChangeColorTemperature(device, "bg_ct", "bg_set_ct_abx") { }