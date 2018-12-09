using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeAmbientBrightness : YeeChangeBrightness
    {
        public YeeChangeAmbientBrightness(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();

        public Task<uint> GetBrightnessAsync(CancellationToken cancellationToken = default)
        {
            return base.GetBrightnessAsync(cancellationToken);
        }

        public Task<JsonValue> SetBrightnessAsync(uint brightness, CancellationToken cancellationToken = default)
        {
            return base.SetBrightnessAsync(brightness, cancellationToken);
        }

        public override Task<JsonValue> SetBrightnessAsync(uint brightness, Effect effect = Effect.Smooth, int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return base.SetBrightnessAsync(brightness, effect, durationMilliseconds, cancellationToken);
        }
    }
}