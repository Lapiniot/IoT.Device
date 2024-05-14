using IoT.Protocol.Upnp;
using IoT.Protocol.Yeelight;
using OOs.Policies;

namespace Yeelight.Control;

internal static class DiscoverCommand
{
    public static async Task RunAsync(IObserver<SsdpReply> observer, CancellationToken stoppingToken)
    {
        var policy = new RepeatPolicyBuilder().WithExponentialInterval(2, 60).Build();
        var keys = new HashSet<string>();
        var enumerator = new YeelightEnumerator(policy);
        await foreach (var reply in enumerator.WithCancellation(stoppingToken).ConfigureAwait(false))
        {
            var location = reply.Location;
            if (keys.Add(location))
            {
                observer.OnNext(reply);
            }
        }
    }
}