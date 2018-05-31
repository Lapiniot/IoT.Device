using System;
using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Soap;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class SystemPropertiesService : SoapActionInvoker
    {
        internal SystemPropertiesService(SoapControlEndpoint endpoint, string deviceId) : base(endpoint,
            new Uri($"{deviceId}/xiaomi.com-SystemProperties-1/control", UriKind.Relative),
            "urn:xiaomi-com:service:SystemProperties:1")
        {
        }

        public async Task<string> GetStringAsync(string variableName, CancellationToken cancellationToken = default)
        {
            return (await InvokeAsync("GetString", cancellationToken, ("VariableName", variableName)).ConfigureAwait(false))["StringValue"];
        }
    }
}