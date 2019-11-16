using System;
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
            return (SwitchState)(await Device.GetPropertyAsync("save_state", cancellationToken).ConfigureAwait(false)).GetInt32();
        }

        public Task SetAutoSaveStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_ps", new[] { "cfg_save_state", ((int)state).ToString() }, cancellationToken);
        }

        public async Task<SwitchState> GetInitPowerStateAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(await Device.GetPropertyAsync("init_power_opt", cancellationToken).ConfigureAwait(false)).GetInt32();
        }

        public Task SetInitPowerStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_ps", new[] { "cfg_init_power", ((int)state + 1).ToString() }, cancellationToken);
        }

        public Task SaveDefaultsAsync(CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_default", EmptyArgs, cancellationToken);
        }
    }
}