using IoT.Protocol.Soap;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class PlaylistService : SoapActionInvoker
    {
        internal PlaylistService(UmiSpeakerDevice parent) : base(parent.Endpoint, "", "")
        {
        }
    }
}