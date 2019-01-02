﻿using System;
using System.Json;
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

        public async Task<JsonArray> CronGetAsync(uint type, CancellationToken cancellationToken = default)
        {
            return (JsonArray)await Device.InvokeAsync("cron_get", new JsonArray { type }, cancellationToken).ConfigureAwait(false);
        }

        public Task<JsonValue> CronAddAsync(uint type, uint delay, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("cron_add", new JsonArray { type, delay }, cancellationToken);
        }

        public Task<JsonValue> CronDelAsync(uint type, CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("cron_del", new JsonArray { type }, cancellationToken);
        }

        public async Task<uint> GetDelayOffAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, "delayoff").ConfigureAwait(false))[0];
        }
    }
}