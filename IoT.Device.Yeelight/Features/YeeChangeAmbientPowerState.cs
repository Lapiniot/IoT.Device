namespace IoT.Device.Yeelight.Features;

public class YeeChangeAmbientPowerState(YeelightDevice device) : YeeChangePowerState(device, "bg_power", "bg_set_power", "bg_toggle")
{
}