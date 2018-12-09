using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeColorRGB : YeelightDeviceFeature
    {
        public new static readonly Type Type = typeof(YeeChangeColorRGB);

        private readonly string propRGB;
        private readonly string propSetRGB;

        public YeeChangeColorRGB(YeelightDevice device) : this(device, "rgb", "set_rgb") { }

        protected YeeChangeColorRGB(YeelightDevice device, string propGet, string propSet) : base(device)
        {
            propRGB = propGet;
            propSetRGB = propSet;
        }

        public override string[] SupportedMethods => new[] { propSetRGB };

        public override string[] SupportedProperties => new[] { propRGB };

        public async Task<uint> GetColorRGBAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, propRGB).ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetColorRGBAsync(uint rgb, CancellationToken cancellationToken = default)
        {
            return SetColorRgbAsync(rgb, Effect.Sudden, 0, cancellationToken);
        }

        public Task<JsonValue> SetColorRgbAsync(uint rgb, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync(propSetRGB, new JsonArray { rgb, effect.ToJsonValue(), durationMilliseconds }, cancellationToken);
        }
    }
}