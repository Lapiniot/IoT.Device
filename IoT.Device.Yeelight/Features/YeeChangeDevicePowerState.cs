namespace IoT.Device.Yeelight.Features;

public class YeeChangeDevicePowerState(YeelightDevice device) : YeeChangePowerState(device, "main_power", "set_main_power", "dev_toggle")
{
}