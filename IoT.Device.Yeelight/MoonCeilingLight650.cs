namespace IoT.Device.Yeelight;

[ExportYeelightDevice("ceiling4")]
[SupportsFeature<ChangeDevicePowerState>, SupportsFeature<ChangeAmbientPowerState>, SupportsFeature<ChangeAmbientBrightness>,
SupportsFeature<ChangeAmbientColorTemperature>, SupportsFeature<ChangeAmbientColorRGB>, SupportsFeature<ChangeAmbientColorHSV>,
SupportsFeature<ProvideAmbientColorMode>, SupportsFeature<SupportsAmbientColorFlowMode>, SupportsFeature<SupportsAmbientScenes>,
SupportsFeature<AdjustAmbientProperty>, SupportsFeature<AdjustAmbientBrightness>, SupportsFeature<AdjustAmbientColorTemperature>,
SupportsFeature<AdjustAmbientColor>, SupportsFeature<SupportsAmbientLight>, SupportsFeature<ProvideLightMode>]
public partial class MoonCeilingLight650(YeelightControlEndpoint endpoint) : YeelightColorLamp(endpoint)
{
    #region Overrides of YeelightDevice

    public override IEnumerable<string> SupportedMethods => [
        "get_prop", "set_default", "set_power", "toggle", "set_bright", "set_scene", "cron_add", "cron_get",
        "cron_del", "start_cf", "stop_cf", "set_ct_abx", "set_name", "set_adjust", "adjust_bright", "adjust_ct",
        "bg_set_rgb", "bg_set_hsv", "bg_set_ct_abx", "bg_start_cf", "bg_stop_cf", "set_scene_bundle", "bg_set_default",
        "bg_set_power", "bg_set_bright", "bg_set_adjust", "bg_adjust_bright", "bg_adjust_color", "bg_adjust_ct",
        "bg_toggle", "dev_toggle"
    ];

    public override IEnumerable<string> SupportedProperties =>
    [
        "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue", "sat", "save_state",
        "flow_params", "nl_br", "nighttime", "miband_sleep", "main_power", "bg_proact", "bg_power", "bg_lmode",
        "bg_bright", "bg_rgb", "bg_hue", "bg_sat", "bg_ct", "init_power_opt", "name", "lan_ctrl"
    ];

    #endregion
}