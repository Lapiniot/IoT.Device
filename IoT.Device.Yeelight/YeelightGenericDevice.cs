using System;
using System.Json;
using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight
{
    public sealed class YeelightGenericDevice : YeelightDevice, IObservable<JsonObject>
    {
        private readonly IObservable<JsonObject> observable;
        private readonly string[] supportedCapabilities;

        public YeelightGenericDevice(YeelightControlEndpoint endpoint) : base(endpoint)
        {
            observable = endpoint;
        }

        public YeelightGenericDevice(YeelightControlEndpoint endpoint, string[] capabilities) : this(endpoint)
        {
            supportedCapabilities = capabilities;
        }

        public override string ModelName { get; } = "yeelight.unknown";

        public override string[] SupportedCapabilities => supportedCapabilities ?? Array.Empty<string>();

        public override string[] SupportedProperties { get; } = Array.Empty<string>();

        public IDisposable Subscribe(IObserver<JsonObject> observer)
        {
            return observable.Subscribe(observer);
        }
    }
}