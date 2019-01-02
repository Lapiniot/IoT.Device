﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeProvideLightMode : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeProvideLightMode);

        public YeeProvideLightMode(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => Array.Empty<string>();

        public override string[] SupportedProperties => new[] { "active_mode" };

        public async Task<LightMode> GetModeAsync(CancellationToken cancellationToken = default)
        {
            return (LightMode)(int)(await Device.GetPropertiesAsync(cancellationToken, "active_mode").ConfigureAwait(false))[0];
        }
    }
}