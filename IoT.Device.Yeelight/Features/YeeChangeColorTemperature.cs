using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public sealed class YeeChangeColorTemperature : YeelightDeviceFeature
    {
        public new static readonly Type Type = typeof(YeeChangeColorTemperature);

        public YeeChangeColorTemperature(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => new[] { "set_ct_abx" };

        public override string[] SupportedProperties => new[] { "ct" };

        public async Task<uint> GetColorTemperatureAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, "ct").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetColorTemperatureAsync(uint temperature, CancellationToken cancellationToken = default)
        {
            return SetColorTemperatureAsync(temperature, Effect.Sudden, 0, cancellationToken);
        }

        public Task<JsonValue> SetColorTemperatureAsync(uint temperature, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_ct_abx", new JsonArray { temperature, effect.ToJsonValue(), durationMilliseconds }, cancellationToken);
        }
    }
}