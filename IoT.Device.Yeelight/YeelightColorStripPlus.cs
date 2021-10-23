using IoT.Device.Yeelight;
using IoT.Device.Yeelight.Features;
using IoT.Protocol.Yeelight;

[assembly: ExportYeelightDevice<YeelightColorStripPlus>("stripe")]

namespace IoT.Device.Yeelight;

[SupportsFeature<YeeChangePowerState>, SupportsFeature<YeeChangeBrightness>, SupportsFeature<YeeChangeColorRGB>,
SupportsFeature<YeeChangeColorHSV>, SupportsFeature<YeeProvideColorMode>, SupportsFeature<YeeSupportsColorFlowMode>,
SupportsFeature<YeeSupportsScenes>, SupportsFeature<YeeAdjustProperty>, SupportsFeature<YeeAdjustBrightness>,
SupportsFeature<YeeAdjustColor>, SupportsFeature<YeeChangeDeviceName>,
SupportsFeature<YeeSupportsSaveState>, SupportsFeature<YeeSupportsCronScheduler>]
public partial class YeelightColorStripPlus : YeelightDevice
{
    public YeelightColorStripPlus(YeelightControlEndpoint endpoint) : base(endpoint) { }

    public override string ModelName { get; } = "yeelink.light.strip2";

    public override IEnumerable<string> SupportedCapabilities => new[]
    {
        "get_prop", "set_default", "set_power", "toggle", "set_bright", "start_cf",
        "stop_cf", "set_scene", "cron_add", "cron_get", "cron_del", "set_rgb",
        "set_hsv", "set_adjust", "adjust_bright", "adjust_color", "set_music", "set_name"
    };

    public override IEnumerable<string> SupportedProperties => new[]
    {
        "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue", "sat",
        "save_state", "flow_params", "init_power_opt", "name", "lan_ctrl"
    };
}