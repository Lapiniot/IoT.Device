using System.Json;
using System.Net;
using IoT.Protocol.Udp;

namespace IoT.Device.Lumi
{
    public class LumiGatewayEnumerator : UdpBroadcastEnumerator<LumiGateway>
    {
        private readonly byte[] whoisMessage;

        public LumiGatewayEnumerator() : base(new IPEndPoint(IPAddress.Parse("224.0.0.50"), 4321))
        {
            whoisMessage = new JsonObject {{"cmd", "whois"}}.Serialize();
        }

        protected override LumiGateway ParseResponse(byte[] buffer, IPEndPoint remoteEp)
        {
            var j = JsonExtensions.Deserialize(buffer);

            return j["cmd"] == "iam" ? new LumiGateway(j["ip"], ushort.Parse(j["port"]), j["sid"]) : null;
        }

        protected override byte[] GetDiscoveryDatagram()
        {
            return Encoding.ASCII.GetBytes("{\"cmd\":\"whois\"}");
        }
    }
}