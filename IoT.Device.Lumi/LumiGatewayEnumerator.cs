using System;
using System.Net;
using IoT.Protocol;
using IoT.Protocol.Lumi;

namespace IoT.Device.Lumi
{
    public class LumiGatewayEnumerator : ConvertingEnumerator<(IPAddress Address, ushort Port, string Sid), LumiGateway>
    {
        public LumiGatewayEnumerator() : base(new LumiEnumerator()) {}

        #region Overrides of ConvertingEnumerator<(IPAddress Address, ushort Port, string Sid),LumiGateway>

        public override LumiGateway Convert((IPAddress Address, ushort Port, string Sid) thing)
        {
            if(thing.Address == null || thing.Sid == string.Empty)
            {
                throw new ApplicationException("Lumi gateway device does not exist or did not respond properly.");
            }

            return new LumiGateway(thing.Address, thing.Port, thing.Sid);
        }

        #endregion
    }
}