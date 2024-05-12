namespace IoT.Device.Yeelight.Features;

public class ChangeDevicePowerState(YeelightDevice device) : ChangePowerState(device, "main_power", "set_main_power", "dev_toggle") { }