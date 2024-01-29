using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

public class YeelightGenericDevice(YeelightControlEndpoint endpoint) : YeelightDevice(endpoint)
{
    private readonly string[] supportedCapabilities;

    public YeelightGenericDevice(YeelightControlEndpoint endpoint, string[] capabilities) : this(endpoint) => supportedCapabilities = capabilities;

    public override string ModelName { get; } = "yeelight.generic";

    public override IEnumerable<string> SupportedMethods => supportedCapabilities ?? [];

    public override IEnumerable<string> SupportedProperties => [];

    public override T GetFeature<T>() => null;
}