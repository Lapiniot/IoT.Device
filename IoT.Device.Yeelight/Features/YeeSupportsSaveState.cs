using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeSupportsSaveState : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeSupportsSaveState);

        public YeeSupportsSaveState(YeelightDevice device) : base(device) { }

        public override IEnumerable<string> SupportedMethods => new[] {"set_ps", "set_default"};

        public override IEnumerable<string> SupportedProperties => new[] {"save_state"};

        public async Task<SwitchState> GetAutoSaveStateAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(await Device.GetPropertyAsync("save_state", cancellationToken).ConfigureAwait(false)).GetInt32();
        }

        public Task SetAutoSaveStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_ps", new[] {"cfg_save_state", ((int)state).ToString(CultureInfo.InvariantCulture)}, cancellationToken);
        }

        public async Task<SwitchState> GetInitPowerStateAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(await Device.GetPropertyAsync("init_power_opt", cancellationToken).ConfigureAwait(false)).GetInt32();
        }

        public Task SetInitPowerStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_ps", new[] {"cfg_init_power", ((int)state + 1).ToString(CultureInfo.InvariantCulture)}, cancellationToken);
        }

        public Task SaveDefaultsAsync(CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_default", EmptyArgs, cancellationToken);
        }
    }
}