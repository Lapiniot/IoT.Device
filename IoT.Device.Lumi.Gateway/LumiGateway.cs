using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CompletionDictionary =
    System.Collections.Concurrent.ConcurrentDictionary<string,
        System.Threading.Tasks.TaskCompletionSource<System.Json.JsonObject>>;
using Cache = IoT.Device.ImplementationCache<
    IoT.Device.Lumi.Gateway.SupportedSubDeviceAttribute,
    IoT.Device.Lumi.Gateway.LumiSubDevice>;
using IoT.Device.Lumi.Gateway.SubDevices;

namespace IoT.Device.Lumi.Gateway
{
    public sealed class LumiGateway : IDisposable
    {
        private object syncRoot = new object();
        private Dictionary<string, LumiSubDevice> children = new Dictionary<string, LumiSubDevice>();
        public LumiGateway(string address, ushort port, string sid)
        {
            Sid = sid;
            client = new CommandDispatchingClient(new IPEndPoint(IPAddress.Parse(address), port));
            listener = new ListeningClient(this, new IPEndPoint(IPAddress.Parse("224.0.0.50"), port));
        }

        public string Sid { get; }
        private readonly CommandDispatchingClient client;
        private readonly ListeningClient listener;
        private bool disposed;

        public void Connect()
        {
            client.Connect();
            listener.Connect();
        }

        public void Close()
        {
            client.Close();
            listener.Close();
        }

        public Task<JsonObject> InvokeAsync(string command, string sid = null,
                CancellationToken cancellationToken = default, params object[] args)
        {
            return client.InvokeAsync(command, sid ?? Sid, cancellationToken, args);
        }

        public async Task<LumiSubDevice[]> GetChildrenAsync(CancellationToken cancellationToken = default)
        {
            var j = await InvokeAsync("get_id_list", Sid, cancellationToken);

            var list = (JsonArray)JsonValue.Parse(j["data"]);

            try
            {
                //Monitor.Enter(syncRoot);

                foreach (string sid in list)
                {
                    var info = await InvokeAsync("read", sid, cancellationToken);
                    var data = JsonValue.Parse(info["data"]) as JsonObject;

                    if (data != null && !children.TryGetValue(sid, out var device))
                    {
                        int id = (int)info["short_id"];
                        string deviceModel = (string)info["model"];
                        device = Cache.CreateInstance(deviceModel, sid, id) ?? new GenericSubDevice(sid, id);
                        device.UpdateState(data);
                        children.Add(sid, device);
                    }
                }
            }
            finally
            {
                //Monitor.Exit(syncRoot);
            }

            return children.Values.ToArray();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                client.Dispose();
                listener.Dispose();
                disposed = true;
            }
        }

        private class CommandDispatchingClient : UdpMessageDispatchingClient
        {
            private readonly CompletionDictionary completions = new CompletionDictionary();
            private readonly IPEndPoint endpoint;

            public CommandDispatchingClient(IPEndPoint remoteEndpoint)
            {
                endpoint = remoteEndpoint;
            }

            public TimeSpan CommandTimeout { get; set; } = TimeSpan.FromSeconds(10);

            protected override UdpClient CreateUdpClient()
            {
                UdpClient client = new UdpClient { EnableBroadcast = false };
                client.Connect(endpoint);
                return client;
            }

            protected override void ProcessResponseBytes(byte[] bytes)
            {
                var json = JsonExtensions.Deserialize(bytes);

                Trace.TraceInformation(json.ToString());

                if (json is JsonObject j && j.TryGetValue((string)"cmd", out var cmd) && j.TryGetValue((string)"sid", out var sid) &&
                    completions.TryRemove(GetCommandKey(GetCmdName(cmd), sid), out var completionSource))
                {
                    completionSource.TrySetResult(j);
                }
            }
            private string GetCommandKey(string command, string sid)
            {
                return $"{command}.{sid}";
            }

            private string GetCmdName(string command)
            {
                return command.EndsWith("_ack") ? command.Substring(0, command.Length - 4) : command;
            }

            public async Task<JsonObject> InvokeAsync(string command, string sid,
                CancellationToken cancellationToken = default, params object[] args)
            {
                var commandKey = GetCommandKey(command, sid);

                var completionSource = new TaskCompletionSource<JsonObject>();

                try
                {
                    var json = new JsonObject { { "cmd", command }, { "sid", sid } };

                    if (!completions.TryAdd(commandKey, completionSource))
                        throw new InvalidOperationException("Error scheduling new command completion task");

                    await SendDatagramAsync(json.Serialize(), cancellationToken).ConfigureAwait(false);

                    using (var timeoutSource = new CancellationTokenSource(CommandTimeout))
                    using (completionSource.Bind(cancellationToken, timeoutSource.Token))
                    {
                        return await completionSource.Task.ConfigureAwait(false);
                    }
                }
                finally
                {
                    completions.TryRemove(commandKey, out var _);
                }
            }
        }

        private class ListeningClient : UdpMessageDispatchingClient
        {
            private readonly LumiGateway gateway;
            private IPEndPoint endpoint;

            public ListeningClient(LumiGateway lumiGateway, IPEndPoint groupEndpoint)
            {
                gateway = lumiGateway;
                endpoint = groupEndpoint;
            }

            protected override UdpClient CreateUdpClient()
            {
                var client = new UdpClient(endpoint.Port);
                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                client.JoinMulticastGroup(endpoint.Address, 64);
                return client;
            }

            protected override void ProcessResponseBytes(byte[] bytes)
            {
                Trace.TraceInformation(Encoding.UTF8.GetString(bytes));
            }
        }
    }
}