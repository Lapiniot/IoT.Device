using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeColorTemperature : YeelightDeviceFeature
    {
        public new static readonly Type Type = typeof(YeeChangeColorTemperature);
        private readonly string propCt;
        private readonly string propSetCt;

        public YeeChangeColorTemperature(YeelightDevice device) : this(device, "ct", "set_ct_abx") { }

        protected YeeChangeColorTemperature(YeelightDevice device, string propGet, string propSet) : base(device)
        {
            propCt = propGet;
            propSetCt = propSet;
        }

        public override string[] SupportedMethods => new[] { propSetCt };

        public override string[] SupportedProperties => new[] { propCt };

        public async Task<uint> GetColorTemperatureAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, propCt).ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetColorTemperatureAsync(uint temperature, CancellationToken cancellationToken = default)
        {
            return SetColorTemperatureAsync(temperature, Effect.Sudden, 0, cancellationToken);
        }

        public Task<JsonValue> SetColorTemperatureAsync(uint temperature, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync(propSetCt, new JsonArray { temperature, effect.ToJsonValue(), durationMilliseconds }, cancellationToken);
        }
    }
}