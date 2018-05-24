using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Soap;

namespace IoT.Device.Xiaomi.Umi.Services
{
    public sealed class SystemPropertiesService : SoapActionInvoker
    {
        internal SystemPropertiesService(UmiSpeakerDevice parent) : base(parent.Endpoint,
            $"{parent.DeviceId}/xiaomi.com-SystemProperties-1/control",
            "urn:xiaomi-com:service:SystemProperties:1")
        {
        }

        public async Task<string> GetStringAsync(string variableName, CancellationToken cancellationToken = default)
        {
            return (await InvokeAsync("GetString", cancellationToken, ("VariableName", variableName)).ConfigureAwait(false))["StringValue"];
        }
    }
}