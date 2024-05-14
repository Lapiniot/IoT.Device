using System.Net;
using System.Text.Json;
using IoT.Protocol.Yeelight;

namespace Yeelight.Control;

internal static partial class MonitorCommand
{
    public static async Task RunAsync(string address, IObserver<JsonElement> observer, CancellationToken stoppingToken)
    {
        var endpoint = new YeelightControlEndpoint(IPAddress.Parse(address));
        await using (endpoint.ConfigureAwait(false))
        {
            await endpoint.ConnectAsync(stoppingToken).ConfigureAwait(false);
            using var subscription = endpoint.Subscribe(observer);
            await Task.Delay(-1, stoppingToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
            await endpoint.DisconnectAsync().ConfigureAwait(false);
        }
    }
}