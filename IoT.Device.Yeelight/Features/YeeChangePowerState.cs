using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangePowerState : YeelightDeviceFeature
    {
        public new static readonly Type Type = typeof(YeeChangePowerState);
        private readonly string propGetPower;
        private readonly string propSetPower;
        private readonly string propSetToggle;

        public YeeChangePowerState(YeelightDevice device) : this(device, "power", "set_power", "toggle") { }

        protected YeeChangePowerState(YeelightDevice device, string propGetPower, string propSetPower, string propSetToggle) :
            base(device)
        {
            this.propGetPower = propGetPower;
            this.propSetPower = propSetPower;
            this.propSetToggle = propSetToggle;
        }

        public override string[] SupportedMethods => new[] { propSetPower, propSetToggle };

        public override string[] SupportedProperties => new[] { propGetPower };

        public async Task<SwitchState> GetPowerStateAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, propGetPower).ConfigureAwait(false))[0]
                .ToEnumValue<SwitchState>();
        }

        public Task<JsonValue> SetPowerStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            return SetPowerStateAsync(state, Effect.Sudden, 0, ColorMode.Normal, cancellationToken);
        }

        public Task<JsonValue> SetPowerStateAsync(SwitchState state = SwitchState.On, Effect effect = Effect.Smooth,
            uint durationMilliseconds = 500, ColorMode mode = ColorMode.Normal,
            CancellationToken cancellationToken = default)
        {
            var args = new JsonArray { state.ToJsonValue(), effect.ToJsonValue(), durationMilliseconds, (int)mode };

            return Device.InvokeAsync(propSetPower, args, cancellationToken);
        }

        public Task<JsonValue> ToggleAsync(CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync(propSetToggle, EmptyArgs, cancellationToken);
        }
    }
}