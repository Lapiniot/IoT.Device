using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public sealed class YeeChangePowerState : YeelightDeviceFeature
    {
        public new static readonly Type Type = typeof(YeeChangePowerState);

        public YeeChangePowerState(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => new[] { "set_power", "toggle" };

        public override string[] SupportedProperties => new[] { "power" };

        public async Task<SwitchState> GetPowerStateAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, "power").ConfigureAwait(false))[0]
                .ToEnumValue<SwitchState>();
        }

        public Task<JsonValue> SetPowerStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_power", new JsonArray { state.ToJsonValue() }, cancellationToken);
        }

        public Task<JsonValue> SetPowerStateAsync(SwitchState state = SwitchState.On, Effect effect = Effect.Smooth,
            uint durationMilliseconds = 500, ColorMode mode = ColorMode.Normal,
            CancellationToken cancellationToken = default)
        {
            var args = new JsonArray { state.ToJsonValue(), effect.ToJsonValue(), durationMilliseconds, (int)mode };

            return Device.InvokeAsync("set_power", args, cancellationToken);
        }

        public Task<JsonValue> ToggleAsync(CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("toggle", EmptyArgs, cancellationToken);
        }
    }
}