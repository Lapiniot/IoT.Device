﻿using System;
using System.Converters;
using System.Net;
using IoT.Protocol;
using IoT.Protocol.Upnp;
using IoT.Protocol.Yeelight;
using YeelightFactory = IoT.Device.Container<IoT.Device.Yeelight.ExportYeelightDeviceAttribute, IoT.Device.Yeelight.YeelightDevice>;

namespace IoT.Device.Yeelight
{
    public class YeelightDeviceEnumerator : ConvertingEnumerator<SsdpReply, YeelightDevice>
    {
        public YeelightDeviceEnumerator() : base(new YeelightEnumerator(), new SsdpReplyComparer("id")) { }

        protected override YeelightDevice Convert(SsdpReply reply)
        {
            if (!reply.TryGetValue("Location", out var location) ||
               !reply.TryGetValue("id", out var value) ||
               !HexConverter.TryParse(value, out uint deviceId))
            {
                return null;
            }

            var uri = new Uri(location);

            var endpoint = new YeelightControlEndpoint(deviceId, new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port));

            try
            {
                var capabilities = reply["support"].Split(' ', ',');

                return YeelightFactory.CreateInstance(reply["model"], endpoint, capabilities) ??
                       new YeelightGenericDevice(endpoint, capabilities);
            }
            catch
            {
                endpoint.Dispose();
                throw;
            }
        }
    }
}