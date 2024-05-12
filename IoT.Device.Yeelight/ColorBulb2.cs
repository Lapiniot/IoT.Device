namespace IoT.Device.Yeelight;

[ExportYeelightDevice("color")]
public sealed partial class ColorBulb2(YeelightControlEndpoint endpoint) : YeelightColorLamp(endpoint)
{
    public override IEnumerable<string> SupportedMethods => [
        "get_prop", "set_default", "set_power", "toggle", "set_bright", "start_cf", "stop_cf", "set_scene", "cron_add",
        "cron_get", "cron_del", "set_ct_abx", "set_rgb", "set_hsv", "set_adjust", "adjust_bright", "adjust_ct",
        "adjust_color", "set_music", "set_name"
    ];

    public override IEnumerable<string> SupportedProperties =>
    [
        "power", "color_mode", "bright", "ct", "rgb", "flowing", "hue",
        "sat", "save_state", "flow_params", "name"
    ];
}