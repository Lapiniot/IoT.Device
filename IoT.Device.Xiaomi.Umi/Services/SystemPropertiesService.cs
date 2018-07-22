﻿using System;
using System.Threading;
using System.Threading.Tasks;
using IoT.Protocol.Soap;
using IoT.Protocol.Upnp.Services;

namespace IoT.Device.Xiaomi.Umi.Services
{
    [ServiceSchema(ServiceSchema)]
    public sealed class SystemPropertiesService : SoapActionInvoker
    {
        public const string ServiceSchema = "urn:xiaomi-com:service:SystemProperties:1";

        public SystemPropertiesService(SoapControlEndpoint endpoint, Uri controlUri) :
            base(endpoint, controlUri, ServiceSchema)
        {
        }

        public SystemPropertiesService(SoapControlEndpoint endpoint) :
            base(endpoint, ServiceSchema)
        {
        }

        public async Task<string> GetStringAsync(string variableName, CancellationToken cancellationToken = default)
        {
            return (await InvokeAsync("GetString", cancellationToken, ("VariableName", variableName)).ConfigureAwait(false))["StringValue"];
        }
    }
}