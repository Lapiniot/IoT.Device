using IoT.Device.Yeelight;
using IoT.Device.Yeelight.Features;
using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

[ExportYeelightDevice("ceiling4")]
[SupportsFeature<YeeChangeDevicePowerState>, SupportsFeature<YeeChangeAmbientPowerState>, SupportsFeature<YeeChangeBrightness>,
SupportsFeature<YeeChangeAmbientBrightness>, SupportsFeature<YeeChangeColorTemperature>, SupportsFeature<YeeChangeAmbientColorTemperature>,
SupportsFeature<YeeChangeColorRGB>, SupportsFeature<YeeChangeAmbientColorRGB>, SupportsFeature<YeeChangeColorHSV>,
SupportsFeature<YeeChangeAmbientColorHSV>, SupportsFeature<YeeProvideColorMode>, SupportsFeature<YeeProvideAmbientColorMode>,
SupportsFeature<YeeSupportsColorFlowMode>, SupportsFeature<YeeSupportsAmbientColorFlowMode>, SupportsFeature<YeeSupportsScenes>,
SupportsFeature<YeeSupportsAmbientScenes>, SupportsFeature<YeeAdjustProperty>, SupportsFeature<YeeAdjustAmbientProperty>,
SupportsFeature<YeeAdjustBrightness>, SupportsFeature<YeeAdjustAmbientBrightness>, SupportsFeature<YeeAdjustColorTemperature>,
SupportsFeature<YeeAdjustAmbientColorTemperature>, SupportsFeature<YeeAdjustColor>, SupportsFeature<YeeAdjustAmbientColor>,
SupportsFeature<YeeSupportsAmbientLight>, SupportsFeature<YeeProvideLightMode>,
SupportsFeature<YeeSupportsCronScheduler>, SupportsFeature<YeeChangeDeviceName>,
SupportsFeature<YeeSupportsSaveState>]
public partial class YeelightMoonCeilingLight650 : YeelightColorLamp
{
    public YeelightMoonCeilingLight650(YeelightControlEndpoint endpoint) : base(endpoint) { }

    #region Overrides of YeelightDevice

    public override string ModelName => "yeelink.light.ceiling4";

    public override IEnumerable<string> SupportedMethods => new[]
    {
        "get_prop", "set_default", "set_power", "toggle", "set_bright", "set_scene", "cron_add",
        "cron_get", "cron_del", "start_cf", "stop_cf", "set_ct_abx", "set_name", "set_adjust",
        "adjust_bright", "adjust_ct", "bg_set_rgb", "bg_set_hsv", "bg_set_ct_abx", "bg_start_cf",
        "bg_stop_cf", "bg_set_scene", "bg_set_default", "bg_set_power", "bg_set_bright", "bg_set_adjust",
        "bg_adjust_bright", "bg_adjust_color", "bg_adjust_ct", "bg_toggle", "dev_toggle"
    };

    public override IEnumerable<string> SupportedProperties => new[]
    {
        "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue", "sat", "save_state",
        "flow_params", "nl_br", "nighttime", "miband_sleep", "main_power", "bg_proact", "bg_power", "bg_lmode",
        "bg_bright", "bg_rgb", "bg_hue", "bg_sat", "bg_ct", "init_power_opt", "name", "lan_ctrl"
    };

    #endregion
}