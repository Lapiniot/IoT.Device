namespace IoT.Device.Yeelight.Features;

public class ChangeAmbientPowerState(YeelightDevice device) : ChangePowerState(device, "bg_power", "bg_set_power", "bg_toggle") { }