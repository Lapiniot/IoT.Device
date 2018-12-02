using System.Json;
using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Interfaces;

namespace IoT.Device.Yeelight
{
    public abstract class YeelightColorLamp : YeelightWhiteLamp
    {
        protected YeelightColorLamp(IConnectedEndpoint<JsonObject, JsonValue> endpoint) : base(endpoint) { }

        public async Task<LightColorMode> GetColorModeAsync(CancellationToken cancellationToken = default)
        {
            return (LightColorMode)(uint)(await GetPropertiesAsync(cancellationToken, "color_mode").ConfigureAwait(false))[0];
        }

        #region Flowing mode settings

        public async Task<SwitchState> GetFlowingStateAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(int)(await GetPropertiesAsync(cancellationToken, "flowing").ConfigureAwait(false))[0];
        }

        public async Task<JsonValue> GetFlowingParamsAsync(CancellationToken cancellationToken = default)
        {
            return (await GetPropertiesAsync(cancellationToken, "flow_params").ConfigureAwait(false))[0];
        }

        #endregion

        #region RGB color mode adjustment

        public async Task<uint> GetColorRGBAsync(CancellationToken cancellationToken = default)
        {
            return (await GetPropertiesAsync(cancellationToken, "rgb").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetColorRGBAsync(uint rgb, CancellationToken cancellationToken = default)
        {
            return SetColorRgbAsync(rgb, Effect.Sudden, 0, cancellationToken);
        }

        public Task<JsonValue> SetColorRgbAsync(uint rgb, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("set_rgb", new JsonArray { rgb, effect.ToJsonValue(), durationMilliseconds }, cancellationToken);
        }

        #endregion

        #region HSV color mode adjustment

        public async Task<uint> GetHueAsync(CancellationToken cancellationToken = default)
        {
            return (await GetPropertiesAsync(cancellationToken, "hue").ConfigureAwait(false))[0];
        }

        public async Task<uint> GetSaturationAsync(CancellationToken cancellationToken = default)
        {
            return (await GetPropertiesAsync(cancellationToken, "sat").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetColorHSVAsync(uint hsv, CancellationToken cancellationToken = default)
        {
            return SetColorHsvAsync(hsv, Effect.Sudden, 0, cancellationToken);
        }

        public Task<JsonValue> SetColorHsvAsync(uint hsv, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("set_hsv", new JsonArray { hsv, effect.ToJsonValue(), durationMilliseconds }, cancellationToken);
        }

        #endregion
    }
}