using System.Globalization;
using System.Text.Json;

namespace IoT.Device.Yeelight.Features;

public class YeeSupportsCronScheduler : YeelightDeviceFeature
{
    public static readonly Type Type = typeof(YeeSupportsCronScheduler);

    public YeeSupportsCronScheduler(YeelightDevice device) : base(device) { }

    public override IEnumerable<string> SupportedMethods => new[] { "cron_get", "cron_add", "cron_del" };

    public override IEnumerable<string> SupportedProperties => Array.Empty<string>();

    public async Task<JsonElement[]> CronGetAsync(uint type, CancellationToken cancellationToken = default) =>
        (await Device.InvokeAsync("cron_get", new[] { type }, cancellationToken).ConfigureAwait(false)).EnumerateArray().ToArray();

    public Task CronAddAsync(uint type, uint delay, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync("cron_add", new[] { type, delay }, cancellationToken);

    public Task CronDelAsync(uint type, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync("cron_del", new[] { type }, cancellationToken);

    public async Task<uint> GetDelayOffAsync(CancellationToken cancellationToken = default) =>
        uint.Parse((await Device.GetPropertyAsync("delayoff", cancellationToken).ConfigureAwait(false)).GetString(), CultureInfo.InvariantCulture);
}