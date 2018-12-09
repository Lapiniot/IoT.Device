using System;
using System.ComponentModel;
using System.Json;
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
            return (SwitchState)(int)(await Device.GetPropertiesAsync(cancellationToken, "bg_proact").ConfigureAwait(false))[0];
        }

        /// <summary>
        /// Sets whether ambient light will be switched on/off with main light
        /// </summary>
        /// <param name="state">On/Off option</param>
        /// <param name="cancellationToken">Token for external cancellation</param>
        /// <returns></returns>
        public Task<JsonValue> SetProactiveModeAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            if (!Enum.IsDefined(typeof(SwitchState), state)) throw new InvalidEnumArgumentException(nameof(state), (int)state, typeof(SwitchState));
            return Device.InvokeAsync("set_ps", new JsonArray { "cfg_bg_proact", ((int)state).ToString() }, cancellationToken);
        }
    }
}