using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeSupportsSaveState : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeSupportsSaveState);

        public YeeSupportsSaveState(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => new[] { "set_ps", "set_default" };

        public override string[] SupportedProperties => new[] { "save_state" };

        public async Task<SwitchState> GetAutoSaveStateAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(int)(await Device.GetPropertiesAsync(cancellationToken, "save_state").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetAutoSaveStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_ps", new JsonArray { "cfg_save_state", ((int)state).ToString() }, cancellationToken);
        }

        public async Task<SwitchState> GetInitPowerStateAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(int)(await Device.GetPropertiesAsync(cancellationToken, "init_power_opt").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetInitPowerStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_ps", new JsonArray { "cfg_init_power", ((int)state + 1).ToString() }, cancellationToken);
        }

        public Task<JsonValue> SetDefaultsAsync(CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_default", EmptyArgs, cancellationToken);
        }
    }
}