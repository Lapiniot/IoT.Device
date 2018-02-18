﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device
{
    /// <summary>
    /// Base abstaract class for IoT devices enumerator which uses network discovery via UDP datagram broadcasting
    /// </summary>
    /// <typeparam name="TThing">Type of the 'thing' discoverable by concrete implementations</typeparam>
    public abstract class UdpBroadcastEnumerator<TThing> where TThing : class
    {
        protected readonly IPEndPoint GroupEndpoint;

        /// <summary>
        /// Type initializer
        /// </summary>
        /// <param name="groupEndpoint">Group endpoint for broadcasting</param>
        protected UdpBroadcastEnumerator(IPEndPoint groupEndpoint)
        {
            GroupEndpoint = groupEndpoint ?? throw new ArgumentNullException(nameof(groupEndpoint));
        }

        /// <summary>
        /// Enumerates network devices by sending discovery datagrams to the <see cref="GroupEndpoint"/> broadcast group
        /// </summary>
        /// <param name="cancellationToken">Cancelation token</param>
        /// <returns>Enumerable sequence of IoT devices that responded to discovery message </returns>
        public IEnumerable<TThing> Enumerate(CancellationToken cancellationToken = default)
        {
            return Enumerate(GroupEndpoint, CreateInstance, cancellationToken);
        }

        /// <summary>
        /// Start enumerating asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to control external cancellation</param>
        /// <returns>Instance of <see cref="AsyncEnumerator{T}"/></returns>
        /// <exception cref="OperationCanceledException">
        /// On cancellation requested via <paramref name="cancellationToken"/>
        /// </exception>
        public async Task<AsyncEnumerator<TThing>> GetEnumeratorAsync(CancellationToken cancellationToken)
        {
            var client = CreateUdpClient(true);

            try
            {
                var datagram = GetDiscoveryDatagram();

                cancellationToken.ThrowIfCancellationRequested();

                await client.SendAsync(datagram, datagram.Length, GroupEndpoint).
                    WaitAndUnwrapAsync(cancellationToken).
                    ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();

                return new UdpAsyncEnumerator<TThing>(client, CreateInstance);
            }
            catch
            {
                client.Dispose();

                throw;
            }
        }

        /// <summary>
        /// Provides generic abstract discovery logic by listening for incoming response datagrams
        /// until <see cref="Timeout"/> time ellapses during receive operation
        /// </summary>
        /// <typeparam name="TResult">Response datagram parsing result</typeparam>
        /// <param name="endpont">Group endpoint for broadcasting</param>
        /// <param name="parser">Raw datagram bytes parser implementation</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Raw datagram parsing result</returns>
        protected IEnumerable<TResult> Enumerate<TResult>(IPEndPoint endpont,
            Func<byte[], IPEndPoint, TResult> parser,
            CancellationToken cancellationToken = default)
        {
            using (var client = CreateUdpClient(true))
            {
                var datagram = GetDiscoveryDatagram();

                if (cancellationToken.IsCancellationRequested) yield break;

                var send = client.SendAsync(datagram, datagram.Length, endpont);

                send.Wait(cancellationToken);

                if (cancellationToken.IsCancellationRequested) yield break;

                while (!cancellationToken.IsCancellationRequested)
                {
                    TResult instance;

                    try
                    {
                        var receive = client.ReceiveAsync();

                        receive.Wait(cancellationToken);

                        if (cancellationToken.IsCancellationRequested) yield break;

                        instance = parser(receive.Result.Buffer, receive.Result.RemoteEndPoint);
                    }
                    catch (OperationCanceledException)
                    {
                        yield break;
                    }
                    catch (Exception exception)
                    {
                        Debug.Write($"Error creating device instance: {exception.Message}", "UDPDeviceEnumerator");

                        // devices returning invalid data should not break enumeration!
                        continue;
                    }

                    yield return instance;
                }
            }
        }

        /// <summary>
        /// Provides generic abstract peer-to-peer discovery for single device hosted at the <paramref name="endpont"/>
        /// </summary>
        /// <typeparam name="TResult">Response datagram parsing result</typeparam>
        /// <param name="endpont">Peer endpoint to be discovered</param>
        /// <param name="parser">Response datagram parser implementation</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response datagram parsing result</returns>
        protected TResult Discover<TResult>(IPEndPoint endpont, Func<byte[], IPEndPoint, TResult> parser,
            CancellationToken cancellationToken = default)
        {
            using (var client = CreateUdpClient(false))
            {
                var datagram = GetDiscoveryDatagram();

                client.SendAsync(datagram, datagram.Length, endpont).Wait(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                var receive = client.ReceiveAsync();

                receive.Wait(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                return parser(receive.Result.Buffer, receive.Result.RemoteEndPoint);
            }
        }

        /// <summary>
        /// Provides generic abstract peer-to-peer discovery for single device hosted at the <paramref name="endpont"/>
        /// </summary>
        /// <typeparam name="TResult">Response datagram parsing result</typeparam>
        /// <param name="endpont">Peer endpoint to be discovered</param>
        /// <param name="parser">Response datagram parser implementation</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Awaitable task that holds response datagram parsing result</returns>
        protected async Task<TResult> DiscoverAsync<TResult>(IPEndPoint endpont,
            Func<byte[], IPEndPoint, TResult> parser, CancellationToken cancellationToken = default)
        {
            using (var client = CreateUdpClient(false))
            {
                var datagram = GetDiscoveryDatagram();

                await client.SendAsync(datagram, datagram.Length, endpont).
                    WaitAndUnwrapAsync(cancellationToken).
                    ConfigureAwait(false);

                var result = await client.ReceiveAsync().
                    WaitAndUnwrapAsync(cancellationToken).
                    ConfigureAwait(false);

                return parser(result.Buffer, result.RemoteEndPoint);
            }
        }

        /// <summary>
        /// Creates and configures instance of the <seealso cref="UdpClient"/> suitable for UDP discovery
        /// </summary>
        /// <param name="enableBroadcast">Should the broadcast mode be enabled</param>
        /// <returns>Instance of <seealso cref="UdpClient"/></returns>
        protected virtual UdpClient CreateUdpClient(bool enableBroadcast)
        {
            return new UdpClient {EnableBroadcast = enableBroadcast};
        }

        /// <summary>
        /// Factory method to create IoT device instance by parsing discovery response datagram bytes
        /// </summary>
        /// <param name="buffer">Discovery message response bytes</param>
        /// <param name="remoteEp">Responder's endpoint information</param>
        /// <returns>
        /// Instance of type
        /// <typeparam name="TThing"></typeparam>
        /// </returns>
        protected abstract TThing CreateInstance(byte[] buffer, IPEndPoint remoteEp);

        /// <summary>
        /// Returns datagram bytes to be send over the network for discovery
        /// </summary>
        /// <returns>Raw datagram bytes</returns>
        protected abstract byte[] GetDiscoveryDatagram();
    }
}