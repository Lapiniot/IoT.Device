using System.Collections.Generic;
using IoT.Device.Yeelight;
using IoT.Device.Yeelight.Features;
using IoT.Protocol.Yeelight;

[assembly: ExportYeelightDevice("color", typeof(YeelightColorBulb2))]

namespace IoT.Device.Yeelight
{
    public class YeelightColorBulb2 : YeelightColorLamp
    {
        private YeeAdjustBrightness abFeature;
        private YeeAdjustColor acFeature;
        private YeeAdjustColorTemperature actFeature;
        private YeeAdjustProperty apFeature;
        private YeeChangeBrightness cbFeature;
        private YeeChangeColorHSV cchsvFeature;
        private YeeProvideColorMode ccmFeature;
        private YeeProvideLightMode plmFeature;
        private YeeChangeColorRGB ccrgbFeature;
        private YeeChangeColorTemperature cctFeature;
        private YeeChangeDeviceName cdnFeature;
        private YeeChangePowerState cpsFeature;
        private YeeSupportsCronScheduler csFeature;
        private YeeSupportsColorFlowMode scfmFeature;
        private YeeSupportsScenes sscFeature;
        private YeeSupportsSaveState sssFeature;

        public YeelightColorBulb2(YeelightControlEndpoint endpoint) : base(endpoint) { }

        public override string ModelName { get; } = "yeelink.light.color2";

        public override IEnumerable<string> SupportedCapabilities => new[]
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

        public override T GetFeature<T>()
        {
            var type = typeof(T);

            if(type == YeeChangePowerState.Type)
            {
                return (cpsFeature ??= new YeeChangePowerState(this)) as T;
            }

            if(type == YeeChangeBrightness.Type)
            {
                return (cbFeature ??= new YeeChangeBrightness(this)) as T;
            }

            if(type == YeeChangeColorTemperature.Type)
            {
                return (cctFeature ??= new YeeChangeColorTemperature(this)) as T;
            }

            if(type == YeeChangeColorRGB.Type)
            {
                return (ccrgbFeature ??= new YeeChangeColorRGB(this)) as T;
            }

            if(type == YeeChangeColorHSV.Type)
            {
                return (cchsvFeature ??= new YeeChangeColorHSV(this)) as T;
            }

            if(type == YeeProvideColorMode.Type)
            {
                return (ccmFeature ??= new YeeProvideColorMode(this)) as T;
            }

            if(type == YeeSupportsCronScheduler.Type)
            {
                return (csFeature ??= new YeeSupportsCronScheduler(this)) as T;
            }

            if(type == YeeSupportsColorFlowMode.Type)
            {
                return (scfmFeature ??= new YeeSupportsColorFlowMode(this)) as T;
            }

            if(type == YeeSupportsScenes.Type)
            {
                return (sscFeature ??= new YeeSupportsScenes(this)) as T;
            }

            if(type == YeeAdjustBrightness.Type)
            {
                return (abFeature ??= new YeeAdjustBrightness(this)) as T;
            }

            if(type == YeeAdjustColorTemperature.Type)
            {
                return (actFeature ??= new YeeAdjustColorTemperature(this)) as T;
            }

            if(type == YeeAdjustColor.Type)
            {
                return (acFeature ??= new YeeAdjustColor(this)) as T;
            }

            if(type == YeeAdjustProperty.Type)
            {
                return (apFeature ??= new YeeAdjustProperty(this)) as T;
            }

            if(type == YeeProvideLightMode.Type)
            {
                return (plmFeature ?? (plmFeature = new YeeProvideLightMode(this))) as T;
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
    }
}