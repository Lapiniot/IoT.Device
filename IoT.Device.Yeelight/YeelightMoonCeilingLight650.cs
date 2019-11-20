using IoT.Device.Yeelight;
using IoT.Device.Yeelight.Features;
using IoT.Protocol.Yeelight;

[assembly: ExportYeelightDevice("ceiling4", typeof(YeelightMoonCeilingLight650))]

namespace IoT.Device.Yeelight
{
    public class YeelightMoonCeilingLight650 : YeelightColorLamp
    {
        private YeeAdjustAmbientBrightness aabFeature;
        private YeeAdjustAmbientColor aacFeature;
        private YeeAdjustAmbientColorTemperature aactFeature;
        private YeeAdjustAmbientProperty aapFeature;
        private YeeAdjustBrightness abFeature;
        private YeeAdjustColor acFeature;
        private YeeAdjustColorTemperature actFeature;
        private YeeAdjustProperty apFeature;
        private YeeChangeAmbientBrightness cabFeature;
        private YeeProvideAmbientColorMode cacmFeature;
        private YeeChangeAmbientColorTemperature cactFeature;
        private YeeChangeAmbientPowerState capsFeature;
        private YeeChangeBrightness cbFeature;
        private YeeChangeAmbientColorHSV ccahsvFeature;
        private YeeChangeAmbientColorRGB ccargbFeature;
        private YeeChangeColorHSV cchsvFeature;
        private YeeProvideColorMode ccmFeature;
        private YeeChangeColorRGB ccrgbFeature;
        private YeeChangeColorTemperature cctFeature;
        private YeeChangeDeviceName cdnFeature;
        private YeeChangeDevicePowerState cdpsFeature;
        private YeeChangePowerState cpsFeature;
        private YeeSupportsCronScheduler csFeature;
        private YeeProvideLightMode plmFeature;
        private YeeSupportsAmbientColorFlowMode sacfmFeature;
        private YeeSupportsAmbientLight salFeature;
        private YeeSupportsAmbientScenes sascFeature;
        private YeeSupportsColorFlowMode scfmFeature;
        private YeeSupportsScenes sscFeature;
        private YeeSupportsSaveState sssFeature;

        public YeelightMoonCeilingLight650(YeelightControlEndpoint endpoint) : base(endpoint) { }

        #region Overrides of YeelightDevice

        public override string ModelName => "yeelink.light.ceiling4";

        public override string[] SupportedCapabilities => new[]
        {
            "get_prop", "set_default", "set_power", "toggle", "set_bright", "set_scene", "cron_add",
            "cron_get", "cron_del", "start_cf", "stop_cf", "set_ct_abx", "set_name", "set_adjust",
            "adjust_bright", "adjust_ct", "bg_set_rgb", "bg_set_hsv", "bg_set_ct_abx", "bg_start_cf",
            "bg_stop_cf", "bg_set_scene", "bg_set_default", "bg_set_power", "bg_set_bright", "bg_set_adjust",
            "bg_adjust_bright", "bg_adjust_color", "bg_adjust_ct", "bg_toggle", "dev_toggle"
        };

        public override string[] SupportedProperties => new[]
        {
            "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue", "sat", "save_state",
            "flow_params", "nl_br", "nighttime", "miband_sleep", "main_power", "bg_proact", "bg_power", "bg_lmode",
            "bg_bright", "bg_rgb", "bg_hue", "bg_sat", "bg_ct", "init_power_opt", "name", "lan_ctrl"
        };

        public override T GetFeature<T>()
        {
            var type = typeof(T);

            if(type == YeeChangeDevicePowerState.Type)
            {
                return (cdpsFeature ??= new YeeChangeDevicePowerState(this)) as T;
            }

            if(type == YeeChangePowerState.Type)
            {
                return (cpsFeature ??= new YeeChangePowerState(this)) as T;
            }

            if(type == YeeChangeAmbientPowerState.Type)
            {
                return (capsFeature ??= new YeeChangeAmbientPowerState(this)) as T;
            }

            if(type == YeeChangeBrightness.Type)
            {
                return (cbFeature ??= new YeeChangeBrightness(this)) as T;
            }

            if(type == YeeChangeAmbientBrightness.Type)
            {
                return (cabFeature ??= new YeeChangeAmbientBrightness(this)) as T;
            }

            if(type == YeeChangeColorTemperature.Type)
            {
                return (cctFeature ??= new YeeChangeColorTemperature(this)) as T;
            }

            if(type == YeeChangeAmbientColorTemperature.Type)
            {
                return (cactFeature ??= new YeeChangeAmbientColorTemperature(this)) as T;
            }

            if(type == YeeChangeColorRGB.Type)
            {
                return (ccrgbFeature ??= new YeeChangeColorRGB(this)) as T;
            }

            if(type == YeeChangeAmbientColorRGB.Type)
            {
                return (ccargbFeature ??= new YeeChangeAmbientColorRGB(this)) as T;
            }

            if(type == YeeChangeColorHSV.Type)
            {
                return (cchsvFeature ??= new YeeChangeColorHSV(this)) as T;
            }

            if(type == YeeChangeAmbientColorHSV.Type)
            {
                return (ccahsvFeature ??= new YeeChangeAmbientColorHSV(this)) as T;
            }

            if(type == YeeProvideColorMode.Type)
            {
                return (ccmFeature ??= new YeeProvideColorMode(this)) as T;
            }

            if(type == YeeProvideAmbientColorMode.Type)
            {
                return (cacmFeature ??= new YeeProvideAmbientColorMode(this)) as T;
            }

            if(type == YeeSupportsCronScheduler.Type)
            {
                return (csFeature ??= new YeeSupportsCronScheduler(this)) as T;
            }

            if(type == YeeSupportsColorFlowMode.Type)
            {
                return (scfmFeature ??= new YeeSupportsColorFlowMode(this)) as T;
            }

            if(type == YeeSupportsAmbientColorFlowMode.Type)
            {
                return (sacfmFeature ??= new YeeSupportsAmbientColorFlowMode(this)) as T;
            }

            if(type == YeeSupportsScenes.Type)
            {
                return (sscFeature ??= new YeeSupportsScenes(this)) as T;
            }

            if(type == YeeSupportsAmbientScenes.Type)
            {
                return (sascFeature ??= new YeeSupportsAmbientScenes(this)) as T;
            }

            if(type == YeeAdjustBrightness.Type)
            {
                return (abFeature ??= new YeeAdjustBrightness(this)) as T;
            }

            if(type == YeeAdjustAmbientBrightness.Type)
            {
                return (aabFeature ??= new YeeAdjustAmbientBrightness(this)) as T;
            }

            if(type == YeeAdjustColorTemperature.Type)
            {
                return (actFeature ??= new YeeAdjustColorTemperature(this)) as T;
            }

            if(type == YeeAdjustAmbientColorTemperature.Type)
            {
                return (aactFeature ??= new YeeAdjustAmbientColorTemperature(this)) as T;
            }

            if(type == YeeAdjustColor.Type)
            {
                return (acFeature ??= new YeeAdjustColor(this)) as T;
            }

            if(type == YeeAdjustAmbientColor.Type)
            {
                return (aacFeature ??= new YeeAdjustAmbientColor(this)) as T;
            }

            if(type == YeeAdjustProperty.Type)
            {
                return (apFeature ??= new YeeAdjustProperty(this)) as T;
            }

            if(type == YeeAdjustAmbientProperty.Type)
            {
                return (aapFeature ??= new YeeAdjustAmbientProperty(this)) as T;
            }

            if(type == YeeSupportsAmbientLight.Type)
            {
                return (salFeature ??= new YeeSupportsAmbientLight(this)) as T;
            }

            if(type == YeeProvideLightMode.Type)
            {
                return (plmFeature ??= new YeeProvideLightMode(this)) as T;
            }

            if(type == YeeChangeDeviceName.Type)
            {
                return (cdnFeature ??= new YeeChangeDeviceName(this)) as T;
            }

            if(type == YeeSupportsSaveState.Type)
            {
                return (sssFeature ??= new YeeSupportsSaveState(this)) as T;
            }

            return null;
        }

        #endregion
    }
}