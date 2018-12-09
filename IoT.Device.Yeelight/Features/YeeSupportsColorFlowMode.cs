using System;
using System.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeSupportsColorFlowMode : YeelightDeviceFeature
    {
        public static Type Type = typeof(YeeSupportsColorFlowMode);

        public YeeSupportsColorFlowMode(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => new[] { "start_cf", "stop_cf" };

        public override string[] SupportedProperties => new[] { "flowing", "flow_params" };

        public async Task<SwitchState> GetFlowingStateAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(int)(await Device.GetPropertiesAsync(cancellationToken, "flowing").ConfigureAwait(false))[0];
        }

        public async Task<JsonValue> GetFlowingParamsAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, "flow_params").ConfigureAwait(false))[0];
        }

        public Task<JsonValue> StartColorFlowAsync(uint count = 0, PostFlowAction mode = PostFlowAction.RestoreState,
            CancellationToken cancellationToken = default,
            params (int Duration, FlowTransition Mode, int Value, int Brightness)[] states)
        {
            var sb = new StringBuilder();

            foreach (var (duration, flowTransition, value, brightness) in states)
            {
                sb.Append(duration);
                sb.Append(",");
                sb.Append((int)flowTransition);
                sb.Append(",");
                sb.Append(value);
                sb.Append(",");
                sb.Append(brightness);
                sb.Append(",");
            }

            if (sb[sb.Length - 1] == ',') sb.Length--;

            return StartColorFlowAsync(count, mode, sb.ToString(), cancellationToken);
        }

        public Task<JsonValue> StartColorFlowAsync(uint count = 0, PostFlowAction mode = PostFlowAction.RestoreState,
            string expression = "", CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("start_cf", new JsonArray(count, (int)mode, expression), cancellationToken);
        }

        public Task<JsonValue> StopColorFlowAsync(CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync("stop_cf", EmptyArgs, cancellationToken);
        }
    }
}