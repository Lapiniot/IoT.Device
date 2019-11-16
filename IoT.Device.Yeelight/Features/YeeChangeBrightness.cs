using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeBrightness : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeChangeBrightness);

        private readonly string propBright;
        private readonly string propSetBright;

        public YeeChangeBrightness(YeelightDevice device) : this(device, "bright", "set_bright") { }

        protected YeeChangeBrightness(YeelightDevice device, string propGet, string propSet) : base(device)
        {
            propBright = propGet;
            propSetBright = propSet;
        }

        public override string[] SupportedMethods => new[] { propSetBright };

        public override string[] SupportedProperties => new[] { propBright };

        public async Task<uint> GetBrightnessAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertyAsync(propBright, cancellationToken).ConfigureAwait(false)).GetUInt32();
        }

        public Task SetBrightnessAsync(uint brightness, CancellationToken cancellationToken = default)
        {
            return SetBrightnessAsync(brightness, Effect.Sudden, 0, cancellationToken);
        }

        public Task SetBrightnessAsync(uint brightness, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync(propSetBright, new object[] { brightness, effect.ToString().ToLowerInvariant(), durationMilliseconds }, cancellationToken);
        }
    }
}