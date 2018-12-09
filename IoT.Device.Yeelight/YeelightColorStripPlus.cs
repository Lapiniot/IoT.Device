using IoT.Device.Yeelight;
using IoT.Device.Yeelight.Features;
using IoT.Protocol.Yeelight;
using YeelightColorStripPlus = IoT.Device.Yeelight.YeelightColorStripPlus;

[assembly: ExportYeelightDevice(0x07C3, typeof(YeelightColorStripPlus), "yeelink.light.strip2")]

namespace IoT.Device.Yeelight
{
    public class YeelightColorStripPlus : YeelightColorLamp
    {
        private YeeChangeBrightness cbFeature;
        private YeeChangePowerState cpsFeature;

        public YeelightColorStripPlus(YeelightControlEndpoint endpoint) : base(endpoint)
        {
        }

        public override string ModelName { get; } = "yeelink.light.strip2";

        public override string[] SupportedCapabilities => new[]
        {
            "get_prop", "set_ps", "set_power", "set_bright", "set_ct_abx", "set_rgb",
            "start_cf", "set_scene", "cron_get", "cron_add", "cron_del", "set_name"
        };

        public override string[] SupportedProperties => new[]
        {
            "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue", "sat",
            "save_state", "flow_params", "nl_br", "nighttime", "miband_sleep", "init_power_opt",
            "lan_ctrl"
        };

        public override T GetFeature<T>()
        {
            var type = typeof(T);

            if (type == YeeChangePowerState.Type)
            {
                return (cpsFeature ?? (cpsFeature = new YeeChangePowerState(this))) as T;
            }

            if (type == YeeChangeBrightness.Type)
            {
                return (cbFeature ?? (cbFeature = new YeeChangeBrightness(this))) as T;
            }

            return null;
        }
    }
}