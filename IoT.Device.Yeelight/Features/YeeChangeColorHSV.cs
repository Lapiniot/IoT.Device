using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public sealed class YeeChangeColorHSV : YeelightDeviceFeature
    {
        public new static readonly Type Type = typeof(YeeChangeColorHSV);

        public YeeChangeColorHSV(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => new[] { "set_hsv" };

        public override string[] SupportedProperties => new[] { "hue", "sat" };

        public async Task<uint> GetHueAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, "hue").ConfigureAwait(false))[0];
        }

        public async Task<uint> GetSaturationAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, "sat").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetColorHSVAsync(uint hsv, CancellationToken cancellationToken = default)
        {
            return SetColorHsvAsync(hsv, Effect.Sudden, 0, cancellationToken);
        }

        public Task<JsonValue> SetColorHsvAsync(uint hsv, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_hsv", new JsonArray { hsv, effect.ToJsonValue(), durationMilliseconds }, cancellationToken);
        }
    }
}