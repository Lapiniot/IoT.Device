using System.Json;
using IoT.Device.Yeelight;
using IoT.Device.Yeelight.Features;
using IoT.Protocol.Interfaces;

[assembly: ExportYeelightDevice(0x0531, typeof(YeelightColorBulb2), "yeelink.light.color2")]

namespace IoT.Device.Yeelight
{
    public class YeelightColorBulb2 : YeelightColorLamp
    {
        private YeeChangeBrightness cbFeature;
        private YeeChangeColorHSV cchsvFeature;
        private YeeChangeColorMode ccmFeature;
        private YeeChangeColorRGB ccrgbFeature;
        private YeeChangeColorTemperature cctFeature;
        private YeeChangeDeviceName cdnFeature;
        private YeeChangePowerState cpsFeature;
        private YeeCronScheduler csFeature;
        private YeeSupportsColorFlowMode scfmFeature;
        private YeeSupportsSaveState sssFeature;

        protected YeelightColorBulb2(IConnectedEndpoint<JsonObject, JsonValue> endpoint) : base(endpoint) { }

        public override string ModelName { get; } = "yeelink.light.color2";

        public override string[] SupportedCapabilities => new[]
        {
            "set_power", "set_bright", "set_ct_abx", "set_rgb", "start_cf",
            "cron_get", "cron_add", "cron_del", "set_name"
        };

        public override string[] SupportedProperties => new[]
        {
            "power", "color_mode", "bright", "ct", "rgb", "flowing", "hue",
            "sat", "save_state", "flow_params", "name"
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

            if (type == YeeChangeColorMode.Type)
            {
                return (ccmFeature ?? (ccmFeature = new YeeChangeColorMode(this))) as T;
            }

            if (type == YeeCronScheduler.Type)
            {
                return (csFeature ?? (csFeature = new YeeCronScheduler(this))) as T;
            }

            if (type == YeeSupportsColorFlowMode.Type)
            {
                return (scfmFeature ?? (scfmFeature = new YeeSupportsColorFlowMode(this))) as T;
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