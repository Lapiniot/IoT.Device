namespace IoT.Device.Yeelight.Features;

public class YeeChangeAmbientColorTemperature : YeeChangeColorTemperature
{
    public YeeChangeAmbientColorTemperature(YeelightDevice device) :
        base(device, "bg_ct", "bg_set_ct_abx")
    { }
}