using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeColorHSV : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeChangeColorHSV);

        private readonly string propHue;
        private readonly string propSaturation;
        private readonly string propSetHSV;

        public YeeChangeColorHSV(YeelightDevice device) : this(device, "hue", "sat", "set_hsv") { }

        protected YeeChangeColorHSV(YeelightDevice device, string propHueGet, string propSaturationGet, string propHSVSet) : base(device)
        {
            propHue = propHueGet;
            propSaturation = propSaturationGet;
            propSetHSV = propHSVSet;
        }

        public override IEnumerable<string> SupportedMethods => new[] {propSetHSV};

        public override IEnumerable<string> SupportedProperties => new[] {propHue, propSaturation};

        public async Task<uint> GetHueAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertyAsync(propHue, cancellationToken).ConfigureAwait(false)).GetUInt32();
        }

        public async Task<uint> GetSaturationAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertyAsync(propSaturation, cancellationToken).ConfigureAwait(false)).GetUInt32();
        }

        public Task SetColorHSVAsync(uint hsv, CancellationToken cancellationToken = default)
        {
            return SetColorHsvAsync(hsv, Effect.Sudden, 0, cancellationToken);
        }

        public Task SetColorHsvAsync(uint hsv, Effect effect = Effect.Smooth, int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync(propSetHSV, new object[] { hsv, effect.ToString().ToLowerInvariant(), durationMilliseconds }, cancellationToken);
        }
    }
}