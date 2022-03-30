namespace IoT.Device.Yeelight.Features;

public class YeeChangeAmbientPowerState : YeeChangePowerState
{
    public static new readonly Type Type = typeof(YeeChangeAmbientPowerState);

    public YeeChangeAmbientPowerState(YeelightDevice device) :
        base(device, "bg_power", "bg_set_power", "bg_toggle")
    { }
}