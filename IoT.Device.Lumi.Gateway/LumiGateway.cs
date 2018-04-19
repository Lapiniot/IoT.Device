using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using IoT.Device.Lumi.Gateway.SubDevices;
using CompletionDictionary =
    System.Collections.Concurrent.ConcurrentDictionary<string,
        System.Threading.Tasks.TaskCompletionSource<System.Json.JsonObject>>;
using Cache = IoT.Device.ImplementationCache<
    IoT.Device.Lumi.Gateway.SupportedSubDeviceAttribute,
    IoT.Device.Lumi.Gateway.LumiSubDevice>;

namespace IoT.Device.Lumi.Gateway
{
    public sealed class LumiGateway : LumiThing
    {
        private readonly Dictionary<string, LumiSubDevice> children;
        private readonly CommandDispatchingClient client;
        private readonly ListeningClient listener;
        private readonly SemaphoreSlim semaphore;
        private bool disposed;
        private int illumination;
        private int rgbValue;
        public string Token { get; private set; }

        public LumiGateway(string address, ushort port, string sid) : base(sid)
        {
            semaphore = new SemaphoreSlim(1, 1);
            children = new Dictionary<string, LumiSubDevice>();
            client = new CommandDispatchingClient(new IPEndPoint(IPAddress.Parse(address), port));
            listener = new ListeningClient(this, new IPEndPoint(IPAddress.Parse("224.0.0.50"), port));
        }

        public int RgbValue
        {
            get => rgbValue;
            private set
            {
                if (rgbValue != value)
                {
                    rgbValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Illumination
        {
            get => illumination;
            private set
            {
                if (illumination != value)
                {
                    illumination = value;
                    OnPropertyChanged();
                }
            }
        }

        public override string ModelName { get; } = "gateway";

        protected override TimeSpan OfflineTimeout { get; } = TimeSpan.FromSeconds(15);

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
            CancellationToken cancellationToken = default)
        {
            return client.InvokeAsync(command, sid ?? Sid, cancellationToken);
        }

        public async Task<LumiSubDevice[]> GetChildrenAsync(CancellationToken cancellationToken = default)
        {
            var j = await InvokeAsync("get_id_list", Sid, cancellationToken);

            var sids = ((JsonArray) JsonValue.Parse(j["data"]) ?? throw new InvalidOperationException())
                .Select(s => (string) s).ToArray();

            await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                var adds = sids.Except(children.Keys).ToArray();
                var removes = children.Keys.Except(sids).ToArray();

                foreach (var sid in adds)
                {
                    var info = await InvokeAsync("read", sid, cancellationToken);

                    if (JsonValue.Parse(info["data"]) is JsonObject data && !children.TryGetValue(sid, out var device))
                    {
                        var id = (int) info["short_id"];
                        var deviceModel = info["model"];
                        device = Cache.CreateInstance((string) deviceModel, sid, id) ?? new GenericSubDevice(sid, id);
                        device.UpdateState(data);
                        children.Add(sid, device);
                    }
                }

                foreach (var sid in removes)
                    if (children.TryGetValue(sid, out var device))
                    {
                        children.Remove(sid);

                        device.Dispose();
                    }
            }
            finally
            {
                semaphore.Release();
            }

            return children.Values.ToArray();
        }

        protected internal override void UpdateState(JsonObject data)
        {
            if (data.TryGetValue("rgb", out var rgb)) RgbValue = rgb;
            if (data.TryGetValue("illumination", out var i)) Illumination = i;
        }

        protected internal override void Heartbeat(JsonObject data)
        {
            base.Heartbeat(data);
            Token = data["token"];
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposed)
            {
                if (disposing)
                {
                    client.Dispose();
                    listener.Dispose();
                    semaphore.Dispose();

                    foreach (var c in children) c.Value.Dispose();
                }

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

            private TimeSpan CommandTimeout { get; } = TimeSpan.FromSeconds(10);

            protected override UdpClient CreateUdpClient()
            {
                var client = new UdpClient {EnableBroadcast = false};
                client.Connect(endpoint);
                return client;
            }

            protected override void ProcessResponseBytes(byte[] bytes)
            {
                var json = JsonExtensions.Deserialize(bytes);

                //Trace.TraceInformation(json.ToString());

                if (json is JsonObject j && j.TryGetValue("cmd", out var cmd) && j.TryGetValue("sid", out var sid) &&
                    completions.TryRemove(GetCommandKey(GetCmdName(cmd), sid), out var completionSource))
                    completionSource.TrySetResult(j);
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
                CancellationToken cancellationToken = default)
            {
                var commandKey = GetCommandKey(command, sid);

                var completionSource = new TaskCompletionSource<JsonObject>();

                try
                {
                    var json = new JsonObject {{"cmd", command}, {"sid", sid}};

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
                    completions.TryRemove(commandKey, out _);
                }
            }
        }

        private class ListeningClient : UdpMessageDispatchingClient
        {
            private readonly IPEndPoint endpoint;
            private readonly LumiGateway gateway;

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
                var json = (JsonObject) JsonExtensions.Deserialize(bytes);

                if (json.TryGetValue("sid", out var sid) && json.TryGetValue("data", out var v) &&
                    JsonValue.Parse(v) is JsonObject data)
                {
                    switch ((string) json["cmd"])
                    {
                        case "report":
                        {
                            if (IsGateway())
                                gateway.UpdateState(data);
                            else if (gateway.children.TryGetValue(sid, out var device)) device.UpdateState(data);
                            break;
                        }
                        case "heartbeat":
                        {
                            if (IsGateway())
                                gateway.Heartbeat(json);
                            else if (gateway.children.TryGetValue(sid, out var device)) device.Heartbeat(data);
                            break;
                        }
                    }

                    bool IsGateway()
                    {
                        return json.TryGetValue("model", out var model) && model == "gateway" && sid == gateway.Sid;
                    }
                }
            }
        }
    }
}