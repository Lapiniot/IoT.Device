using System;
using System.Collections.Generic;
using System.Net;
using System.Policies;
using IoT.Protocol;
using IoT.Protocol.Lumi;

namespace IoT.Device.Lumi
{
    public class LumiGatewayEnumerator : ConvertingEnumerator<(IPAddress Address, int Port, string Sid), LumiGateway>
    {
        public LumiGatewayEnumerator(IRepeatPolicy discoveryPolicy) :
            base(new LumiEnumerator(discoveryPolicy), new LumiDeviceComparer()) {}

        #region Overrides of ConvertingEnumerator<(IPAddress Address, ushort Port, string Sid),LumiGateway>

        protected override LumiGateway Convert((IPAddress Address, int Port, string Sid) thing)
        {
            if(thing.Address == null || string.IsNullOrEmpty(thing.Sid))
            {
                throw new InvalidOperationException("Lumi gateway device does not exist or did not respond properly.");
            }

            return new LumiGateway(thing.Address, thing.Port, thing.Sid);
        }

        #endregion

        private class LumiDeviceComparer : IEqualityComparer<(IPAddress Address, int Port, string Sid)>
        {
            #region Implementation of IEqualityComparer<in (IPAddress Address, ushort Port, string Sid)>

            public bool Equals((IPAddress Address, int Port, string Sid) x, (IPAddress Address, int Port, string Sid) y)
            {
                return StringComparer.OrdinalIgnoreCase.Equals(x.Sid, y.Sid);
            }

            public int GetHashCode((IPAddress Address, int Port, string Sid) obj)
            {
                return obj.Sid?.GetHashCode(StringComparison.InvariantCulture) ?? 0;
            }

            #endregion
        }
    }
}