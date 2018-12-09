using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeBrightness : YeelightDeviceFeature
    {
        public new static readonly Type Type = typeof(YeeChangeBrightness);
        public YeeChangeBrightness(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => new[] { "set_bright" };

        public override string[] SupportedProperties => new[] { "bright" };


        public virtual async Task<uint> GetBrightnessAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, "bright").ConfigureAwait(false))[0];
        }

        public virtual Task<JsonValue> SetBrightnessAsync(uint brightness, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_bright", new JsonArray(brightness), cancellationToken);
        }

        public virtual Task<JsonValue> SetBrightnessAsync(uint brightness, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_bright", new JsonArray { brightness, effect.ToJsonValue(), durationMilliseconds }, cancellationToken);
        }
    }
}