using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeDeviceName : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeChangeDeviceName);

        public YeeChangeDeviceName(YeelightDevice device) : base(device) { }

        public override IEnumerable<string> SupportedMethods => new[] {"set_name"};

        public override IEnumerable<string> SupportedProperties => new[] {"name"};

        public async Task<string> GetNameAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertyAsync("name", cancellationToken).ConfigureAwait(false)).GetString();
        }

        public Task SetNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_name", new[] {name}, cancellationToken);
        }
    }
}