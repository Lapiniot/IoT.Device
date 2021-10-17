using System;
using System.Collections.Generic;
using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight
{
    public class YeelightGenericDevice : YeelightDevice
    {
        private readonly string[] supportedCapabilities;

        public YeelightGenericDevice(YeelightControlEndpoint endpoint) : base(endpoint)
        { }

        public YeelightGenericDevice(YeelightControlEndpoint endpoint, string[] capabilities) : this(endpoint)
        {
            supportedCapabilities = capabilities;
        }

        public override string ModelName { get; } = "yeelight.generic";

        public override IEnumerable<string> SupportedCapabilities => supportedCapabilities ?? Array.Empty<string>();

        public override IEnumerable<string> SupportedProperties { get; } = Array.Empty<string>();

        public override T GetFeature<T>()
        {
            return null;
        }
    }
}