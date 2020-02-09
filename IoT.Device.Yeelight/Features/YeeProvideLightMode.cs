using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeProvideLightMode : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeProvideLightMode);

        public YeeProvideLightMode(YeelightDevice device) : base(device) { }

        public override IEnumerable<string> SupportedMethods => Array.Empty<string>();

        public override IEnumerable<string> SupportedProperties => new[] {"active_mode"};

        public async Task<LightMode> GetModeAsync(CancellationToken cancellationToken = default)
        {
            return (LightMode)(await Device.GetPropertyAsync("active_mode", cancellationToken).ConfigureAwait(false)).GetInt32();
        }
    }
}