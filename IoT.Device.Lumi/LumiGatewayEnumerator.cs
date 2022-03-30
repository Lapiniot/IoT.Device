using System.Policies;
using IoT.Protocol;
using IoT.Protocol.Lumi;

namespace IoT.Device.Lumi;

public class LumiGatewayEnumerator : ConvertingEnumerator<LumiEndpoint, LumiGateway>
{
    public LumiGatewayEnumerator(IRepeatPolicy discoveryPolicy) :
        base(new LumiEnumerator(discoveryPolicy), new LumiDeviceComparer())
    { }

    #region Overrides of ConvertingEnumerator<(IPAddress Address, ushort Port, string Sid),LumiGateway>

    protected override LumiGateway Convert(LumiEndpoint thing)
    {
        return thing.EndPoint is not null && !string.IsNullOrEmpty(thing.Sid)
            ? new LumiGateway(thing.EndPoint, thing.Sid)
            : throw new InvalidDataException("Lumi gateway device does not exist or did not respond properly.");
    }

    #endregion

    private class LumiDeviceComparer : IEqualityComparer<LumiEndpoint>
    {
        #region Implementation of IEqualityComparer<in (IPAddress Address, ushort Port, string Sid)>

        public bool Equals(LumiEndpoint x, LumiEndpoint y) => StringComparer.OrdinalIgnoreCase.Equals(x.Sid, y.Sid);

        public int GetHashCode(LumiEndpoint obj) => obj.Sid?.GetHashCode(StringComparison.InvariantCulture) ?? 0;

        #endregion
    }
}