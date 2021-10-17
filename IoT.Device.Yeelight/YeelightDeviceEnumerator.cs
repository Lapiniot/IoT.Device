﻿using System;
using System.Net;
using System.Policies;
using IoT.Protocol;
using IoT.Protocol.Upnp;
using IoT.Protocol.Yeelight;
using System.Globalization;
using static System.Globalization.NumberStyles;
using YeelightFactory = IoT.Device.Container<IoT.Device.Yeelight.ExportYeelightDeviceAttribute, IoT.Device.Yeelight.YeelightDevice>;

namespace IoT.Device.Yeelight
{
    public class YeelightDeviceEnumerator : ConvertingEnumerator<SsdpReply, YeelightDevice>
    {
        public YeelightDeviceEnumerator(IRepeatPolicy discoveryPolicy) :
            base(new YeelightEnumerator(discoveryPolicy), new SsdpReplyComparer("id"))
        { }

        protected override YeelightDevice Convert(SsdpReply thing)
        {
            if(thing is null) throw new ArgumentNullException(nameof(thing));

            if(!thing.TryGetValue("Location", out var location) ||
               !thing.TryGetValue("id", out var value) ||
               !TryParseNumber(value, out uint deviceId))
            {
                return null;
            }

            var uri = new Uri(location);

            var endpoint = new YeelightControlEndpoint(deviceId, new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port));

            try
            {
                var capabilities = thing["support"].Split(' ', ',');

                return YeelightFactory.CreateInstance(thing["model"], endpoint, capabilities) ??
                    new YeelightGenericDevice(endpoint, capabilities);
            }
            catch
            {
                var _ = endpoint.DisposeAsync();
                throw;
            }
        }

        private static bool TryParseNumber(string s, out uint result)
        {
            return s.StartsWith("0x", false, CultureInfo.InvariantCulture) && uint.TryParse(s[2..], HexNumber, null, out result) ||
                   uint.TryParse(s, Integer & ~AllowLeadingSign, null, out result);
        }
    }
}