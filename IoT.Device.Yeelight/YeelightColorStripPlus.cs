using System;
using System.Json;
using IoT.Device.Yeelight;
using IoT.Protocol.Yeelight;
using YeelightColorStripPlus = IoT.Device.Yeelight.YeelightColorStripPlus;

[assembly: ExportYeelightDevice(0x07C3, typeof(YeelightColorStripPlus), "yeelink.light.strip2")]

namespace IoT.Device.Yeelight
{
    public sealed class YeelightColorStripPlus : YeelightColorLamp, IObservable<JsonObject>
    {
        private readonly YeelightControlEndpoint observable;
        private readonly string[] supportedCapabilities;

        public YeelightColorStripPlus(YeelightControlEndpoint endpoint) : base(endpoint)
        {
            observable = endpoint;
        }

        public YeelightColorStripPlus(YeelightControlEndpoint endpoint, string[] capabilities) : this(endpoint)
        {
            supportedCapabilities = capabilities;
        }

        public override string ModelName { get; } = "yeelink.light.strip2";

        public override string[] SupportedCapabilities => new[]
        {
            "get_prop", "set_ps", "set_power", "set_bright", "set_ct_abx", "set_rgb",
            "start_cf", "set_scene",
            "cron_get", "cron_add", "cron_del", "set_name"
        };

        public override string[] SupportedProperties => new[]
        {
            "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue", "sat",
            "save_state", "flow_params", "nl_br", "nighttime", "miband_sleep", "init_power_opt",
            "lan_ctrl"
        };

        public IDisposable Subscribe(IObserver<JsonObject> observer)
        {
            return observable.Subscribe(observer);
        }
    }
}