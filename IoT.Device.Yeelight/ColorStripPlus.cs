using IoT.Device.Yeelight.Features;
using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

[ExportYeelightDevice("stripe")]
[SupportsFeature<ChangePowerState>, SupportsFeature<ChangeBrightness>, SupportsFeature<ChangeColorRGB>,
SupportsFeature<ChangeColorHSV>, SupportsFeature<ProvideColorMode>, SupportsFeature<SupportsColorFlowMode>,
SupportsFeature<SupportsScenes>, SupportsFeature<AdjustProperty>, SupportsFeature<AdjustBrightness>,
SupportsFeature<AdjustColor>, SupportsFeature<ChangeDeviceName>,
SupportsFeature<SupportsSaveState>, SupportsFeature<SupportsCronScheduler>]
public partial class ColorStripPlus(YeelightControlEndpoint endpoint) : YeelightDevice(endpoint)
{
    public override IEnumerable<string> SupportedMethods =>
    [
        "get_prop", "set_default", "set_power", "toggle", "set_bright", "start_cf",
        "stop_cf", "set_scene", "cron_add", "cron_get", "cron_del", "set_rgb",
        "set_hsv", "set_adjust", "adjust_bright", "adjust_color", "set_music", "set_name"
    ];

    public override IEnumerable<string> SupportedProperties =>
    [
        "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue", "sat",
        "save_state", "flow_params", "init_power_opt", "name", "lan_ctrl"
    ];
}