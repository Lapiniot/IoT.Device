using System.Text.Json;
using IoT.Protocol.Yeelight;
using OOs;

namespace IoT.Device.Yeelight;

public abstract class YeelightDevice(YeelightControlEndpoint endpoint) : IConnectedObject, IAsyncDisposable
{
    public YeelightControlEndpoint Endpoint => endpoint;

    public abstract string ModelName { get; }

    public abstract IEnumerable<string> SupportedMethods { get; }

    public abstract IEnumerable<string> SupportedProperties { get; }

    public abstract T GetFeature<T>() where T : YeelightDeviceFeature;

    public Task<JsonElement> InvokeAsync(Command message, CancellationToken cancellationToken) =>
        Endpoint.InvokeAsync(message, cancellationToken);

    public Task<JsonElement> InvokeAsync(string method, object args, CancellationToken cancellationToken) =>
        InvokeAsync(new(method, args), cancellationToken);

    public async Task<JsonElement[]> GetPropertiesAsync(string[] properties, CancellationToken cancellationToken = default) =>
        [.. (await InvokeAsync("get_prop", properties.Cast<object>().ToArray(), cancellationToken).ConfigureAwait(false)).EnumerateArray()];

    public async Task<T> GetPropertyAsync<T>(string property, CancellationToken cancellationToken = default) where T : struct, Enum
    {
        var element = await InvokeAsync("get_prop", new object[] { property }, cancellationToken).ConfigureAwait(false);

        return Enum.Parse<T>(element[0].GetString(), true);
    }

    public async Task<JsonElement> GetPropertyAsync(string property, CancellationToken cancellationToken = default) =>
        (await InvokeAsync("get_prop", new object[] { property }, cancellationToken).ConfigureAwait(false))[0];

    public override string ToString() => Endpoint.ToString();

    #region Implementation of IConnectedObject

    public bool IsConnected => Endpoint.IsConnected;

    public Task ConnectAsync(CancellationToken cancellationToken = default) => Endpoint.ConnectAsync(cancellationToken);

    public Task DisconnectAsync() => Endpoint.DisconnectAsync();

    #endregion

    #region Implementation of IAsyncDisposable

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return Endpoint.DisposeAsync();
    }

    #endregion
}