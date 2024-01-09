using IoT.Protocol;
using IoT.Protocol.Lumi;
using OOs.Policies;

namespace IoT.Device.Lumi;

public class LumiGatewayEnumerator(IRepeatPolicy discoveryPolicy) : ConvertingEnumerator<LumiEndpoint, LumiGateway>(new LumiEnumerator(discoveryPolicy), new LumiDeviceComparer())
{
    #region Overrides of ConvertingEnumerator<(IPAddress Address, ushort Port, string Sid),LumiGateway>

    protected override LumiGateway Convert(LumiEndpoint thing) =>
        thing.EndPoint is not null && !string.IsNullOrEmpty(thing.Sid)
            ? new LumiGateway(thing.EndPoint, thing.Sid)
            : throw new InvalidDataException("Lumi gateway device does not exist or did not respond properly.");

    #endregion

    private sealed class LumiDeviceComparer : IEqualityComparer<LumiEndpoint>
    {
        #region Implementation of IEqualityComparer<in (IPAddress Address, ushort Port, string Sid)>

        public bool Equals(LumiEndpoint x, LumiEndpoint y) => StringComparer.OrdinalIgnoreCase.Equals(x.Sid, y.Sid);

        public int GetHashCode(LumiEndpoint obj) => obj.Sid?.GetHashCode(StringComparison.InvariantCulture) ?? 0;

        #endregion
    }
}