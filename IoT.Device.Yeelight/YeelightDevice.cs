using System;
using System.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Interfaces;

namespace IoT.Device.Yeelight
{
    public abstract class YeelightDevice : AsyncConnectedObject
    {
        protected JsonArray EmptyArgs = new JsonArray();

        protected YeelightDevice(IConnectedEndpoint<JsonObject, JsonValue> endpoint)
        {
            Endpoint = endpoint;
        }

        public IConnectedEndpoint<JsonObject, JsonValue> Endpoint { get; }

        public abstract string ModelName { get; }

        public abstract string[] SupportedCapabilities { get; }

        public abstract string[] SupportedProperties { get; }

        public abstract T GetFeature<T>() where T : YeelightDeviceFeature;

        public async Task<JsonValue> InvokeAsync(JsonObject message, CancellationToken cancellationToken)
        {
            var response = await Endpoint.InvokeAsync(message, cancellationToken).ConfigureAwait(false);

            if (response is JsonObject json)
            {
                if (json.TryGetValue("result", out var result)) return result;

                if (json.TryGetValue("error", out var e)) throw new YeelightException(e["code"], e["message"]);

                return json;
            }

            throw new InvalidOperationException("Invalid response received (not a valid JSON object)");
        }

        public Task<JsonValue> InvokeAsync(string method, JsonObject args,
            CancellationToken cancellationToken)
        {
            return InvokeAsync(new JsonObject { { "method", method }, { "params", args } }, cancellationToken);
        }

        public Task<JsonValue> InvokeAsync(string method, JsonArray args,
            CancellationToken cancellationToken)
        {
            return InvokeAsync(new JsonObject { { "method", method }, { "params", args } }, cancellationToken);
        }

        public async Task<JsonArray> GetPropertiesAsync(CancellationToken cancellationToken = default,
            params string[] properties)
        {
            return await InvokeAsync("get_prop",
                new JsonArray(properties.Select(p => (JsonValue)p)),
                cancellationToken).ConfigureAwait(false) as JsonArray;
        }

        public override string ToString()
        {
            return Endpoint.ToString();
        }

        protected override Task OnConnectAsync(CancellationToken cancellationToken)
        {
            return Endpoint.ConnectAsync(cancellationToken);
        }

        protected override Task OnDisconnectAsync()
        {
            return Endpoint.DisconnectAsync();
        }
    }
}