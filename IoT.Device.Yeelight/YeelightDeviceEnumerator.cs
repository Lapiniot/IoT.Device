using System;
using System.Converters;
using IoT.Protocol;
using IoT.Protocol.Upnp;
using IoT.Protocol.Yeelight;
using Container = IoT.Device.Container<IoT.Device.Yeelight.ExportYeelightDeviceAttribute, IoT.Device.Yeelight.YeelightDevice>;

namespace IoT.Device.Yeelight
{
    public sealed class YeelightDeviceEnumerator : ConvertingEnumerator<SsdpReply, YeelightDevice>
    {
        public YeelightDeviceEnumerator() : base(new YeelightEnumerator()) { }

        public override YeelightDevice Convert(SsdpReply reply)
        {
            if (reply.TryGetValue("Location", out var location) &&
               reply.TryGetValue("id", out var value) &&
               HexConverter.TryParse(value, out uint deviceId))
            {
                var endpoint = new YeelightControlEndpoint(deviceId, new Uri(location));

                var capabilities = reply["support"].Split(' ', ',');

                return Container.CreateInstance(deviceId >> 16, endpoint, capabilities) ??
                       new YeelightGenericDevice(endpoint, capabilities);
            }

            return null;
        }
    }
}