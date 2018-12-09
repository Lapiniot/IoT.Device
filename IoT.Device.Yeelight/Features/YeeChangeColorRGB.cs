using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public sealed class YeeChangeColorRGB : YeelightDeviceFeature
    {
        public new static readonly Type Type = typeof(YeeChangeColorRGB);

        public YeeChangeColorRGB(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => new[] { "set_rgb" };

        public override string[] SupportedProperties => new[] { "rgb" };

        public async Task<uint> GetColorRGBAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, "rgb").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetColorRGBAsync(uint rgb, CancellationToken cancellationToken = default)
        {
            return SetColorRgbAsync(rgb, Effect.Sudden, 0, cancellationToken);
        }

        public Task<JsonValue> SetColorRgbAsync(uint rgb, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_rgb", new JsonArray { rgb, effect.ToJsonValue(), durationMilliseconds }, cancellationToken);
        }
    }
}