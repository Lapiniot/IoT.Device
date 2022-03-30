namespace IoT.Device.Yeelight.Features;

public class YeeChangeAmbientColorTemperature : YeeChangeColorTemperature
{
    public static new readonly Type Type = typeof(YeeChangeAmbientColorTemperature);

    public YeeChangeAmbientColorTemperature(YeelightDevice device) :
        base(device, "bg_ct", "bg_set_ct_abx")
    { }
}