using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device
{
    public abstract class UdpEndpoint
    {
        private readonly object syncRoot = new object();
        private volatile UdpClient client;
        private bool disposed;

        protected UdpEndpoint(IPEndPoint endpoint)
        {
            Endpoint = endpoint;
        }

        protected IPEndPoint Endpoint { get; }

        /// <summary>
        /// Disposes object instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }


        /// <summary>
        /// Establishes logical connection with IoT device: sets up udp client and starts listening for incoming datagrams.
        /// Must be called before control message sending.
        /// </summary>
        public void Connect()
        {
            CheckDisposed();

            if(client == null)
            {
                lock(syncRoot)
                {
                    if(client == null)
                    {
                        client = CreateUdpClient();

                        OnConnect();
                    }
                }
            }
        }

        /// <summary>
        /// Closes logical connection: stops listening for incoming datagrams and frees up udp client allocated resources.
        /// </summary>
        public void Close()
        {
            lock(syncRoot)
            {
                OnClose();

                client?.Dispose();

                client = null;
            }
        }

        protected abstract UdpClient CreateUdpClient();

        protected abstract void OnConnect();

        protected abstract void OnClose();

        protected void CheckConnected()
        {
            if(client == null) throw new InvalidOperationException("Not connected");
        }

        protected void CheckDisposed()
        {
            if(disposed) throw new ObjectDisposedException("this");
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(!disposed)
                {
                    Close();

                    disposed = true;
                }
            }
        }

        protected Task SendDatagramAsync(byte[] datagram, CancellationToken cancellationToken)
        {
            CheckConnected();
            CheckDisposed();

            return client.SendAsync(datagram, datagram.Length).WaitAndUnwrapAsync(cancellationToken);
        }

        protected Task<UdpReceiveResult> ReceiveDatagramAsync(CancellationToken cancellationToken)
        {
            CheckConnected();
            CheckDisposed();

            return client.ReceiveAsync().WaitAndUnwrapAsync(cancellationToken);
        }
    }
}