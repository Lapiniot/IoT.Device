using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Interfaces
{
    public interface ICommunicationEndpoint<in TRequest, TResponse> : IDisposable
    {
        Task<TResponse> InvokeAsync(TRequest message, CancellationToken cancellationToken);

        void Connect();

        void Close();
    }
}