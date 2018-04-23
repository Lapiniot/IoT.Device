using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device
{
    public abstract class UdpListener : UdpEndpoint
    {
        private CancellationTokenSource cancellationTokenSource;

        protected UdpListener(IPEndPoint endpoint) : base(endpoint)
        {
        }

        protected override void OnConnect()
        {
            cancellationTokenSource = new CancellationTokenSource();

            var token = cancellationTokenSource.Token;

            Task.Run(() => DispatchAsync(token), token);
        }

        protected override void OnClose()
        {
            cancellationTokenSource?.Cancel();

            cancellationTokenSource?.Dispose();

            cancellationTokenSource = null;
        }

        private async Task DispatchAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = await ReceiveDatagramAsync(cancellationToken);

                    OnDataAvailable(result.RemoteEndPoint, result.Buffer);
                }
                catch (OperationCanceledException)
                {
                    Trace.TraceInformation("Cancelling message dispatching loop...");
                }
                catch (Exception e)
                {
                    Trace.TraceError($"Error in mesaage dispatch: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Process response datagram bytes
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        /// <param name="bytes">Raw datagram bytes</param>
        protected abstract void OnDataAvailable(IPEndPoint remoteEndPoint, byte[] bytes);
    }
}