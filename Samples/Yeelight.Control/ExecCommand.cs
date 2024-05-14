using System.Net;
using System.Text.Json;
using IoT.Device.Yeelight;
using IoT.Protocol.Yeelight;

namespace Yeelight.Control;

internal static class ExecCommand
{
    public static async Task<JsonElement> RunAsync(string address, string command, string paramsJson)
    {
        var endpoint = new YeelightControlEndpoint(IPAddress.Parse(address));
        await using (endpoint.ConfigureAwait(false))
        {
            var device = new YeelightGenericDevice(endpoint);
            await using (device.ConfigureAwait(false))
            {
                await device.ConnectAsync().ConfigureAwait(false);
                var @params = JsonSerializer.Deserialize(paramsJson, JsonContext.Default.JsonNode);
                var result = await device.InvokeAsync(new Command(command, @params), CancellationToken.None).ConfigureAwait(false);
                await device.DisconnectAsync().ConfigureAwait(false);
                return result;
            }
        }
    }
}