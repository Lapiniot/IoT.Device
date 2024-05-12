namespace IoT.Device.Yeelight;

[ExportYeelightDevice("mono5")]
public sealed partial class FilamentBulbST64(YeelightControlEndpoint endpoint) : YeelightWhiteLamp(endpoint)
{
    public override IEnumerable<string> SupportedMethods => [
        "get_prop", "set_default", "set_power", "toggle", "set_bright", "set_scene", "cron_add", "cron_get",
        "cron_del", "start_cf", "stop_cf", "set_name", "set_adjust", "adjust_bright"
    ];

    public override IEnumerable<string> SupportedProperties =>
    [
        "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue", "sat",
        "save_state", "flow_params", "init_power_opt", "name", "lan_ctrl"
    ];
}