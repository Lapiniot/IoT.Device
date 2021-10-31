using IoT.Device.Yeelight.Features;
using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

[ExportYeelightDevice("color")]
[SupportsFeature<YeeChangePowerState>, SupportsFeature<YeeChangeBrightness>, SupportsFeature<YeeChangeColorTemperature>,
SupportsFeature<YeeChangeColorRGB>, SupportsFeature<YeeChangeColorHSV>, SupportsFeature<YeeProvideColorMode>,
SupportsFeature<YeeAdjustProperty>, SupportsFeature<YeeAdjustBrightness>, SupportsFeature<YeeAdjustColorTemperature>,
SupportsFeature<YeeAdjustColor>, SupportsFeature<YeeSupportsColorFlowMode>, SupportsFeature<YeeSupportsScenes>,
SupportsFeature<YeeSupportsCronScheduler>, SupportsFeature<YeeChangeDeviceName>,
SupportsFeature<YeeSupportsSaveState>]
public partial class YeelightColorBulb2 : YeelightColorLamp
{
    public YeelightColorBulb2(YeelightControlEndpoint endpoint) : base(endpoint) { }

    public override IEnumerable<string> SupportedMethods => new[]
    {
        "get_prop", "set_default", "set_power", "toggle", "set_bright", "start_cf",
        "stop_cf", "set_scene", "cron_add", "cron_get", "cron_del", "set_ct_abx", "set_rgb",
        "set_hsv", "set_adjust", "adjust_bright", "adjust_ct", "adjust_color", "set_music", "set_name"
    };

    public override IEnumerable<string> SupportedProperties => new[]
    {
        "power", "color_mode", "bright", "ct", "rgb", "flowing", "hue",
        "sat", "save_state", "flow_params", "name"
    };
}