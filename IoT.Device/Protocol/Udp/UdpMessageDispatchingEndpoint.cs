using System.Net;
using System.Threading;
using System.Threading.Tasks;
using IoT.Device.Interfaces;

namespace IoT.Device.Protocol.Udp
{
    /// <summary>
    /// Base abstract type for IoT device controlled via UDP datagram messaging with dispatching queue support.
    /// </summary>
    public abstract class UdpMessageDispatchingEndpoint<TRequest, TResponse> :
        UdpListener, ICommunicationEndpoint<TRequest, TResponse>
    {
        protected UdpMessageDispatchingEndpoint(IPEndPoint endpoint) : base(endpoint)
        {
        }

        public abstract Task<TResponse> InvokeAsync(TRequest message, CancellationToken cancellationToken);
    }
}