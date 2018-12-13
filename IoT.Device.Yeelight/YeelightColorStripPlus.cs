using System.Json;
using System.Net;
using IoT.Device.Yeelight;
using IoT.Device.Yeelight.Features;
using IoT.Protocol.Interfaces;

[assembly: ExportYeelightDevice(0x07C3, typeof(YeelightColorStripPlus), "yeelink.light.strip2")]

namespace IoT.Device.Yeelight
{
    public class YeelightColorStripPlus : YeelightDevice
    {
        private YeeAdjustBrightness abFeature;
        private YeeAdjustColor acFeature;
        private YeeAdjustColorTemperature actFeature;
        private YeeAdjustProperty apFeature;
        private YeeChangeBrightness cbFeature;
        private YeeChangeColorHSV cchsvFeature;
        private YeeProvideColorMode ccmFeature;
        private YeeChangeColorRGB ccrgbFeature;
        private YeeChangeColorTemperature cctFeature;
        private YeeChangeDeviceName cdnFeature;
        private YeeChangePowerState cpsFeature;
        private YeeSupportsCronScheduler csFeature;
        private YeeProvideLightMode plmFeature;
        private YeeSupportsColorFlowMode scfmFeature;
        private YeeSupportsScenes sscFeature;
        private YeeSupportsSaveState sssFeature;

        public YeelightColorStripPlus(IConnectedEndpoint<JsonObject, JsonValue> endpoint) : base(endpoint) { }

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

            if (type == YeeChangeColorTemperature.Type)
            {
                return (cctFeature ?? (cctFeature = new YeeChangeColorTemperature(this))) as T;
            }

            if (type == YeeChangeColorRGB.Type)
            {
                return (ccrgbFeature ?? (ccrgbFeature = new YeeChangeColorRGB(this))) as T;
            }

            if (type == YeeChangeColorHSV.Type)
            {
                return (cchsvFeature ?? (cchsvFeature = new YeeChangeColorHSV(this))) as T;
            }

            if (type == YeeProvideColorMode.Type)
            {
                return (ccmFeature ?? (ccmFeature = new YeeProvideColorMode(this))) as T;
            }

            if (type == YeeSupportsCronScheduler.Type)
            {
                return (csFeature ?? (csFeature = new YeeSupportsCronScheduler(this))) as T;
            }

            if (type == YeeSupportsColorFlowMode.Type)
            {
                return (scfmFeature ?? (scfmFeature = new YeeSupportsColorFlowMode(this))) as T;
            }

            if (type == YeeSupportsScenes.Type)
            {
                return (sscFeature ?? (sscFeature = new YeeSupportsScenes(this))) as T;
            }

            if (type == YeeAdjustBrightness.Type)
            {
                return (abFeature ?? (abFeature = new YeeAdjustBrightness(this))) as T;
            }

            if (type == YeeAdjustColorTemperature.Type)
            {
                return (actFeature ?? (actFeature = new YeeAdjustColorTemperature(this))) as T;
            }

            if (type == YeeAdjustColor.Type)
            {
                return (acFeature ?? (acFeature = new YeeAdjustColor(this))) as T;
            }

            if (type == YeeAdjustProperty.Type)
            {
                return (apFeature ?? (apFeature = new YeeAdjustProperty(this))) as T;
            }

            if (type == YeeProvideLightMode.Type)
            {
                return (plmFeature ?? (plmFeature = new YeeProvideLightMode(this))) as T;
            }

            if (type == YeeChangeDeviceName.Type)
            {
                return (cdnFeature ?? (cdnFeature = new YeeChangeDeviceName(this))) as T;
            }

            if (type == YeeSupportsSaveState.Type)
            {
                return (sssFeature ?? (sssFeature = new YeeSupportsSaveState(this))) as T;
            }

            return null;
        }
    }
}