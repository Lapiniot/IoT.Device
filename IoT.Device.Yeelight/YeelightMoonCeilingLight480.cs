using System.Json;
using IoT.Device.Yeelight;
using IoT.Device.Yeelight.Features;
using IoT.Protocol.Interfaces;

[assembly: ExportYeelightDevice("ceiling3", typeof(YeelightMoonCeilingLight480))]

namespace IoT.Device.Yeelight
{
    public class YeelightMoonCeilingLight480 : YeelightWhiteLamp
    {
        private YeeAdjustBrightness abFeature;
        private YeeAdjustColorTemperature actFeature;
        private YeeAdjustProperty apFeature;
        private YeeChangeBrightness cbFeature;
        private YeeProvideColorMode ccmFeature;
        private YeeChangeColorTemperature cctFeature;
        private YeeChangeDeviceName cdnFeature;
        private YeeChangePowerState cpsFeature;
        private YeeSupportsCronScheduler csFeature;
        private YeeProvideLightMode plmFeature;
        private YeeSupportsScenes sscFeature;
        private YeeSupportsSaveState sssFeature;

        public YeelightMoonCeilingLight480(IConnectedEndpoint<JsonObject, JsonValue> endpoint) : base(endpoint) {}

        #region Overrides of YeelightDevice

        public override string ModelName => "yeelink.light.ceiling3";

        public override string[] SupportedCapabilities => new[]
        {
            "get_prop", "set_default", "set_power", "toggle", "set_bright", "set_scene",
            "cron_add", "cron_get", "cron_del", "start_cf", "stop_cf", "set_ct_abx",
            "set_name", "set_adjust", "adjust_bright", "adjust_ct"
        };

        public override string[] SupportedProperties => new[]
        {
            "power", "color_mode", "bright", "ct", "flowing", "pdo_status", "save_state", "flow_params",
            "nl_br", "nighttime", "miband_sleep", "init_power_opt", "name", "lan_ctrl"
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

            if (type == YeeProvideColorMode.Type)
            {
                return (ccmFeature ?? (ccmFeature = new YeeProvideColorMode(this))) as T;
            }

            if (type == YeeSupportsCronScheduler.Type)
            {
                return (csFeature ?? (csFeature = new YeeSupportsCronScheduler(this))) as T;
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

        #endregion
    }
}