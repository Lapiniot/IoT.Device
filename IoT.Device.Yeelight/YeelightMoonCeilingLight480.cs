using IoT.Device.Yeelight.Features;
using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

[ExportYeelightDevice("ceiling3")]
[SupportsFeature<YeeChangePowerState>, SupportsFeature<YeeChangeBrightness>, SupportsFeature<YeeChangeColorTemperature>,
SupportsFeature<YeeProvideLightMode>, SupportsFeature<YeeProvideColorMode>, SupportsFeature<YeeSupportsScenes>,
SupportsFeature<YeeAdjustProperty>, SupportsFeature<YeeAdjustBrightness>,
SupportsFeature<YeeAdjustColorTemperature>, SupportsFeature<YeeSupportsCronScheduler>, SupportsFeature<YeeChangeDeviceName>,
SupportsFeature<YeeSupportsSaveState>]
public partial class YeelightMoonCeilingLight480(YeelightControlEndpoint endpoint) : YeelightWhiteLamp(endpoint)
{
    #region Overrides of YeelightDevice

    public override IEnumerable<string> SupportedMethods =>
    [
        "get_prop", "set_default", "set_power", "toggle", "set_bright", "set_scene",
        "cron_add", "cron_get", "cron_del", "start_cf", "stop_cf", "set_ct_abx",
        "set_name", "set_adjust", "adjust_bright", "adjust_ct"
    ];

    public override IEnumerable<string> SupportedProperties =>
    [
        "power", "color_mode", "bright", "ct", "flowing", "pdo_status", "save_state", "flow_params",
        "nl_br", "nighttime", "miband_sleep", "init_power_opt", "name", "lan_ctrl"
    ];

    #endregion
}