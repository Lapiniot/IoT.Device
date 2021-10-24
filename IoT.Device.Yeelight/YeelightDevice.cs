using System.Text.Json;
using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

public abstract class YeelightDevice : IConnectedObject, IAsyncDisposable
{
    public IReadOnlyDictionary<string, object> EmptyArgs { get; } = new Dictionary<string, object>();

    protected YeelightDevice(YeelightControlEndpoint endpoint)
    {
        Endpoint = endpoint;
    }

    public YeelightControlEndpoint Endpoint { get; }

    public abstract string ModelName { get; }

    public abstract IEnumerable<string> SupportedMethods { get; }

    public abstract IEnumerable<string> SupportedProperties { get; }

    public abstract T GetFeature<T>() where T : YeelightDeviceFeature;

    public async Task<JsonElement> InvokeAsync(RequestMessage message, CancellationToken cancellationToken)
    {
        var json = await Endpoint.InvokeAsync(message, cancellationToken).ConfigureAwait(false);

        if(json.TryGetProperty("result", out var result)) return result;

        if(json.TryGetProperty("error", out var e)) throw new YeelightException(e.GetProperty("code").GetInt32(), e.GetProperty("message").GetString());

        return json;
    }

    public Task<JsonElement> InvokeAsync(string method, object args, CancellationToken cancellationToken)
    {
        return InvokeAsync(new RequestMessage(method, args), cancellationToken);
    }

    public async Task<JsonElement[]> GetPropertiesAsync(string[] properties, CancellationToken cancellationToken = default)
    {
        return (await InvokeAsync("get_prop", properties.Cast<object>().ToArray(), cancellationToken).ConfigureAwait(false)).EnumerateArray().ToArray();
    }

    public async Task<T> GetPropertyAsync<T>(string property, CancellationToken cancellationToken = default) where T : struct, Enum
    {
        var element = await InvokeAsync("get_prop", new object[] { property }, cancellationToken).ConfigureAwait(false);

        return Enum.Parse<T>(element[0].GetString(), true);
    }

    public async Task<JsonElement> GetPropertyAsync(string property, CancellationToken cancellationToken = default)
    {
        return (await InvokeAsync("get_prop", new object[] { property }, cancellationToken).ConfigureAwait(false))[0];
    }

    public override string ToString()
    {
        return Endpoint.ToString();
    }

    #region Implementation of IConnectedObject

    public bool IsConnected => Endpoint.IsConnected;

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        return Endpoint.ConnectAsync(cancellationToken);
    }

    public Task DisconnectAsync()
    {
        return Endpoint.DisconnectAsync();
    }

    #endregion

    #region Implementation of IAsyncDisposable

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return Endpoint.DisposeAsync();
    }

    #endregion
}