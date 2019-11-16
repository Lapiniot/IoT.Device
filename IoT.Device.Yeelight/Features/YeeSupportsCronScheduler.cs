using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeSupportsCronScheduler : YeelightDeviceFeature
    {
        public static Type Type = typeof(YeeSupportsCronScheduler);

        public YeeSupportsCronScheduler(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => new[] { "cron_get", "cron_add", "cron_del" };

        public override string[] SupportedProperties => Array.Empty<string>();

        public async Task<JsonElement[]> CronGetAsync(uint type, CancellationToken cancellationToken = default)
        {
            return (await Device.InvokeAsync("cron_get", new[] {type}, cancellationToken).ConfigureAwait(false)).EnumerateArray().ToArray();
        }

        public Task CronAddAsync(uint type, uint delay, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("cron_add", new[] {type, delay}, cancellationToken);
        }

        public Task CronDelAsync(uint type, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("cron_del", new[] {type}, cancellationToken);
        }

        public async Task<uint> GetDelayOffAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertyAsync("delayoff", cancellationToken).ConfigureAwait(false)).GetUInt32();
        }
    }
}