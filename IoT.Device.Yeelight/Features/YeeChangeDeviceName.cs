using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeDeviceName : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeChangeDeviceName);

        public YeeChangeDeviceName(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => new[] { "set_name" };

        public override string[] SupportedProperties => new[] { "name" };

        public async Task<string> GetNameAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, "name").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_name", new JsonArray { name }, cancellationToken);
        }
    }
}