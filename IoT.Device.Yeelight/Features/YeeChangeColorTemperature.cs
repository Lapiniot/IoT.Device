using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeColorTemperature : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeChangeColorTemperature);
        private readonly string propCt;
        private readonly string propSetCt;

        public YeeChangeColorTemperature(YeelightDevice device) : this(device, "ct", "set_ct_abx") { }

        protected YeeChangeColorTemperature(YeelightDevice device, string propGet, string propSet) : base(device)
        {
            propCt = propGet;
            propSetCt = propSet;
        }

        public override IEnumerable<string> SupportedMethods => new[] {propSetCt};

        public override IEnumerable<string> SupportedProperties => new[] {propCt};

        public async Task<uint> GetColorTemperatureAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertyAsync(propCt, cancellationToken).ConfigureAwait(false)).GetUInt32();
        }

        public Task SetColorTemperatureAsync(uint temperature, CancellationToken cancellationToken = default)
        {
            return SetColorTemperatureAsync(temperature, Effect.Sudden, 0, cancellationToken);
        }

        public Task SetColorTemperatureAsync(uint temperature, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync(propSetCt, new object[] { temperature, effect.ToString().ToLowerInvariant(), durationMilliseconds }, cancellationToken);
        }
    }
}