using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device
{
    /// <summary>
    ///     Base abstract type for IoT device controlled via UDP datagram messaging with dispatching queue support.
    /// </summary>
    public abstract class UdpMessageDispatchingClient : IDisposable
    {
        private readonly object syncRoot = new object();
        private CancellationTokenSource cancellationTokenSource;
        protected volatile UdpClient Client;
        private bool disposed;

        /// <summary>
        ///     Disposes object instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        ///     Establishes logical connection with IoT device: sets up udp client and starts listening for incoming datagrams.
        ///     Must be called before control message sending.
        /// </summary>
        public void Connect()
        {
            CheckDisposed();

            if (Client == null)
                lock (syncRoot)
                {
                    if (Client == null)
                    {
                        Client = CreateUdpClient();

                        cancellationTokenSource = new CancellationTokenSource();

                        var token = cancellationTokenSource.Token;

                        Task.Run(() => DispatchAsync(token), token);
                    }
                }
        }

        protected abstract UdpClient CreateUdpClient();

        /// <summary>
        ///     Closes logical connection: stops listening for incoming datagrams and frees up udp client allocated resources.
        /// </summary>
        public void Close()
        {
            lock (syncRoot)
            {
                cancellationTokenSource?.Cancel();

                cancellationTokenSource?.Dispose();

                Client?.Dispose();

                Client = null;

                cancellationTokenSource = null;
            }
        }

        private async Task DispatchAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
                try
                {
                    var result = await Client.ReceiveAsync().WaitAndUnwrapAsync(cancellationToken)
                        .ConfigureAwait(false);

                    ProcessResponseBytes(result.Buffer);
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

        /// <summary>
        ///     Sends raw udp datagram over network
        /// </summary>
        /// <param name="datagram">Raw datagram</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Awaitable task</returns>
        protected async Task SendDatagramAsync(byte[] datagram, CancellationToken cancellationToken = default)
        {
            CheckDisposed();
            CheckConnected();

            await Client.SendAsync(datagram, datagram.Length).WaitAndUnwrapAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        ///     Process response datagram bytes
        /// </summary>
        /// <param name="bytes">Raw datagram bytes</param>
        protected abstract void ProcessResponseBytes(byte[] bytes);

        protected void CheckConnected()
        {
            if (Client == null) throw new InvalidOperationException("Not connected");
        }

        protected void CheckDisposed()
        {
            if (disposed) throw new ObjectDisposedException("this");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                Close();

                disposed = true;
            }
        }
    }
}