using System;
using System.Json;
using IoT.Device.Yeelight;
using IoT.Protocol.Yeelight;
using YeelightColorBulb2 = IoT.Device.Yeelight.YeelightColorBulb2;

[assembly: ExportYeelightDevice(0x0531, typeof(YeelightColorBulb2), "yeelink.light.color2")]

namespace IoT.Device.Yeelight
{
    public sealed class YeelightColorBulb2 : YeelightColorLamp, IObservable<JsonObject>
    {
        private readonly YeelightControlEndpoint observable;
        private readonly string[] supportedCapabilities;

        public YeelightColorBulb2(YeelightControlEndpoint endpoint) : base(endpoint)
        {
            observable = endpoint;
        }

        public YeelightColorBulb2(YeelightControlEndpoint endpoint, string[] capabilities) : this(endpoint)
        {
            supportedCapabilities = capabilities;
        }

        public override string ModelName { get; } = "yeelink.light.color2";

        public override string[] SupportedCapabilities => new[]
        {
            "set_power", "set_bright", "set_ct_abx", "set_rgb", "start_cf",
            "cron_get", "cron_add", "cron_del", "set_name"
        };

        public override string[] SupportedProperties => new[]
        {
            "power", "color_mode", "bright", "ct", "rgb", "flowing", "pdo_status", "hue",
            "sat", "save_state", "flow_params", "name", "lan_ctrl"
        };

        public override T GetFeature<T>()
        {
            return null;
        }

        public IDisposable Subscribe(IObserver<JsonObject> observer)
        {
            return observable.Subscribe(observer);
        }
    }
}