using IoT.Protocol.Soap;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class PlaylistService : SoapActionInvoker
    {
        internal PlaylistService(UmiSpeakerDevice parent) : base(parent.Endpoint,
            $"{parent.DeviceId}-MR/xiaomi.com-Playlist-1/control",
            "urn:xiaomi-com:service:Playlist:1")
        {
        }
    }
}