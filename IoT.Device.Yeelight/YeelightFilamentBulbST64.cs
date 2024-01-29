using IoT.Device.Yeelight.Features;
using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

[ExportYeelightDevice("mono5")]
[SupportsFeature<YeeChangePowerState>, SupportsFeature<YeeChangeBrightness>, SupportsFeature<YeeProvideColorMode>,
SupportsFeature<YeeSupportsColorFlowMode>, SupportsFeature<YeeSupportsScenes>, SupportsFeature<YeeAdjustProperty>,
SupportsFeature<YeeAdjustBrightness>, SupportsFeature<YeeSupportsCronScheduler>,
SupportsFeature<YeeChangeDeviceName>, SupportsFeature<YeeSupportsSaveState>]
public partial class YeelightFilamentBulbST64(YeelightControlEndpoint endpoint) : YeelightWhiteLamp(endpoint)
{
    public override IEnumerable<string> SupportedMethods =>
    [
        "get_prop", "set_default", "set_power", "toggle", "set_bright", "start_cf", "stop_cf", "set_scene",
        "cron_add", "cron_get", "cron_del", "set_adjust", "adjust_bright", "set_name"
    ];

    public override IEnumerable<string> SupportedProperties =>
    [
        "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue", "sat",
        "save_state", "flow_params", "init_power_opt", "name", "lan_ctrl"
    ];
}