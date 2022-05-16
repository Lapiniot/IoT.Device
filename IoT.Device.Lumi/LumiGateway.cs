using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using IoT.Device.Metadata;
using IoT.Protocol.Lumi;
using static System.Text.Json.JsonSerializer;
using static System.Text.Json.JsonValueKind;
using static System.TimeSpan;
using static IoT.Device.Metadata.PowerSource;
using static IoT.Device.Metadata.ConnectivityTypes;
using Factory = IoT.Device.DeviceFactory<IoT.Device.Lumi.LumiSubDevice>;
using IoT.Device.Lumi.SubDevices;

namespace IoT.Device.Lumi;

[ModelID("DGNWG02LM")]
[PowerSource(Plugged)]
[ConnectivityType(WiFi24 | ZigBee)]
public sealed class LumiGateway : LumiThing, IConnectedObject, IObserver<JsonElement>
{
    private readonly Dictionary<string, LumiSubDevice> children;
    private readonly LumiControlEndpoint client;
    private readonly LumiEventListener listener;
    private readonly SemaphoreSlim semaphore;
    private readonly IDisposable subscription;
    private int disposed;
    private int illumination;
    private int rgbValue;

    public LumiGateway(IPEndPoint endpoint, string sid) : base(sid)
    {
        ArgumentNullException.ThrowIfNull(endpoint);

        semaphore = new SemaphoreSlim(1, 1);
        children = new Dictionary<string, LumiSubDevice>();
        client = new LumiControlEndpoint(endpoint);
        listener = new LumiEventListener(new IPEndPoint(IPAddress.Parse("224.0.0.50"), endpoint.Port));
        subscription = listener.Subscribe(this);
    }

    public string Token { get; private set; }

    public int RgbValue
    {
        get => rgbValue;
        private set => Set(ref rgbValue, value);
    }

    public int Illumination
    {
        get => illumination;
        private set => Set(ref illumination, value);
    }

    public override string ModelName { get; } = "gateway";

    // Gateway sends heartbeats every 10 seconds.
    // We give extra 2 seconds to the timeout value.
    protected override TimeSpan HeartbeatTimeout { get; } = FromSeconds(10) + FromSeconds(2);

    public bool IsConnected { get; private set; }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (!IsConnected)
        {
            await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                if (!IsConnected)
                {
                    await client.ConnectAsync(cancellationToken).ConfigureAwait(false);
                    await listener.ConnectAsync(cancellationToken).ConfigureAwait(false);
                    IsConnected = true;
                }
            }
            finally
            {
                _ = semaphore.Release();
            }
        }
    }

    public async Task DisconnectAsync()
    {
        if (IsConnected)
        {
            await semaphore.WaitAsync().ConfigureAwait(false);

            try
            {
                if (IsConnected)
                {
                    await client.DisconnectAsync().ConfigureAwait(false);
                    await listener.DisconnectAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                IsConnected = false;

                _ = semaphore.Release();
            }
        }
    }

    public Task<JsonElement> InvokeAsync(string command, string sid = null, CancellationToken cancellationToken = default) =>
        client.InvokeAsync(command, sid ?? Sid, cancellationToken);

    public async IAsyncEnumerable<LumiSubDevice> GetChildrenAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var json = await InvokeAsync("get_id_list", Sid, cancellationToken).ConfigureAwait(false);

        var data = Deserialize<JsonElement>(json.GetProperty("data").GetString() ?? string.Empty);

        var sids = data.EnumerateArray().Select(a => a.GetString()).ToArray();

        await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            var adds = sids.Except(children.Keys).ToArray();
            var removes = children.Keys.Except(sids).ToArray();

            foreach (var sid in removes)
            {
                if (!children.TryGetValue(sid, out var device)) continue;

                _ = children.Remove(sid);

                await device.DisposeAsync().ConfigureAwait(false);
            }

            foreach (var (_, value) in children)
            {
                yield return value;
            }

            foreach (var sid in adds)
            {
                var info = await InvokeAsync("read", sid, cancellationToken).ConfigureAwait(false);

                if (!info.TryGetProperty("data", out var d) || children.TryGetValue(sid, out var device)) continue;

                var id = info.GetProperty("short_id").GetInt32();
                var deviceModel = info.GetProperty("model").GetString();
#pragma warning disable CA1508 // CA1508: Avoid dead conditional code - probably false noise from code analyzer
                device = Factory.Create(deviceModel, sid, id) ?? new GenericSubDevice(sid, id);
#pragma warning restore CA1508
                device.OnStateChanged(Deserialize<JsonElement>(d.GetString() ?? string.Empty));
                children.Add(sid, device);
                yield return device;
            }
        }
        finally
        {
            _ = semaphore.Release();
        }
    }

    protected internal override void OnStateChanged(JsonElement state)
    {
        if (state.TryGetProperty("rgb", out var value) && value.ValueKind == Number)
        {
            RgbValue = value.GetInt32();
        }

        if (state.TryGetProperty("illumination", out value) && value.ValueKind == Number)
        {
            Illumination = value.GetInt32();
        }
    }

    #region Overrides of LumiThing

    public override async ValueTask DisposeAsync()
    {
        if (Interlocked.CompareExchange(ref disposed, 1, 0) != 0) return;

        await base.DisposeAsync().ConfigureAwait(false);

        try
        {
            try
            {
                using (subscription)
                using (semaphore) { }

                foreach (var c in children)
                {
                    await using (c.Value.ConfigureAwait(false)) { }
                }

                children.Clear();
            }
            finally
            {
                await listener.DisposeAsync().ConfigureAwait(false);
            }
        }
        finally
        {
            await client.DisposeAsync().ConfigureAwait(false);
        }
    }

    #endregion

    #region Implementation of IObserver<in JsonElement>

    void IObserver<JsonElement>.OnCompleted() { }

    void IObserver<JsonElement>.OnError(Exception error) { }

    void IObserver<JsonElement>.OnNext(JsonElement message)
    {
        if (!message.TryGetProperty("sid", out var sid) ||
           !message.TryGetProperty("cmd", out var command) ||
           !message.TryGetProperty("data", out var v) ||
           !(Deserialize<JsonElement>(v.GetString() ?? string.Empty) is { } data))
        {
            return;
        }

        var key = sid.GetString();

        if (string.IsNullOrEmpty(key)) return;

        switch (command.GetString())
        {
            case "heartbeat":
                {
                    if (key == Sid)
                    {
                        OnHeartbeat(data);
                        Token = message.GetProperty("token").GetString();
                    }
                    else if (children.TryGetValue(key, out var device))
                    {
                        device.OnHeartbeat(data);
                    }
                }

                break;
            case "report":
                {
                    if (key == Sid)
                    {
                        OnStateChanged(data);
                    }
                    else if (children.TryGetValue(key, out var device))
                    {
                        device.OnStateChanged(data);
                    }
                }

                break;
        }
    }

    #endregion
}