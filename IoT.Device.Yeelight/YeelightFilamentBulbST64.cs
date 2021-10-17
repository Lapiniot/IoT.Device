using IoT.Device.Yeelight;
using IoT.Device.Yeelight.Features;
using IoT.Protocol.Yeelight;

[assembly: ExportYeelightDevice<YeelightFilamentBulbST64>("mono5")]

namespace IoT.Device.Yeelight
{
    public class YeelightFilamentBulbST64 : YeelightWhiteLamp
    {
        private YeeChangePowerState cpsFeature;
        private YeeChangeBrightness cbFeature;
        private YeeProvideColorMode ccmFeature;
        private YeeSupportsCronScheduler csFeature;
        private YeeSupportsColorFlowMode scfmFeature;
        private YeeSupportsScenes sscFeature;
        private YeeAdjustBrightness abFeature;
        private YeeAdjustProperty apFeature;
        private YeeChangeDeviceName cdnFeature;
        private YeeSupportsSaveState sssFeature;

        public YeelightFilamentBulbST64(YeelightControlEndpoint endpoint) : base(endpoint)
        { }

        public override string ModelName { get; } = "yeelink.light.mono5";

        public override IEnumerable<string> SupportedCapabilities => new[]
        {
            "get_prop", "set_default", "set_power", "toggle", "set_bright", "start_cf", "stop_cf", "set_scene",
            "cron_add", "cron_get", "cron_del", "set_adjust", "adjust_bright", "set_name"
        };

        public override IEnumerable<string> SupportedProperties => new[]
        {
            "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue", "sat",
            "save_state", "flow_params", "init_power_opt", "name", "lan_ctrl"
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

            if(type == YeeAdjustProperty.Type)
            {
                return (apFeature ??= new YeeAdjustProperty(this)) as T;
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