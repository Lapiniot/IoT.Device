using System.Json;
using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Interfaces;

namespace IoT.Device.Yeelight
{
    public abstract class YeelightWhiteLamp : YeelightLamp
    {
        protected YeelightWhiteLamp(IConnectedEndpoint<JsonObject, JsonValue> endpoint) : base(endpoint) { }

        public async Task<uint> GetColorTemperatureAsync(CancellationToken cancellationToken = default)
        {
            return (await GetPropertiesAsync(cancellationToken, "ct").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> SetColorTemperatureAsync(uint temperature, CancellationToken cancellationToken = default)
        {
            return SetColorTemperatureAsync(temperature, Effect.Sudden, 0, cancellationToken);
        }

        public Task<JsonValue> SetColorTemperatureAsync(uint temperature, Effect effect = Effect.Smooth,
            int durationMilliseconds = 500, CancellationToken cancellationToken = default)
        {
            return InvokeAsync("set_ct_abx", new JsonArray { temperature, effect.ToJsonValue(), durationMilliseconds }, cancellationToken);
        }
    }
}