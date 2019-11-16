using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeSupportsAmbientLight : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeSupportsAmbientLight);

        public YeeSupportsAmbientLight(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => new[] { "set_ps" };

        public override string[] SupportedProperties => new[] { "bg_proact" };

        public async Task<SwitchState> GetProactiveModeAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(await Device.GetPropertyAsync("bg_proact", cancellationToken).ConfigureAwait(false)).GetInt32();
        }

        /// <summary>
        /// Sets whether ambient light will be switched on/off with main light
        /// </summary>
        /// <param name="state">On/Off option</param>
        /// <param name="cancellationToken">Token for external cancellation</param>
        /// <returns></returns>
        public Task SetProactiveModeAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("set_ps", new[] { "cfg_bg_proact", ((int)state).ToString() }, cancellationToken);
        }
    }
}