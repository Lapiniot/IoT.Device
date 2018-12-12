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

        private readonly string propFlowing;
        private readonly string propFlowParams;
        private readonly string startSet;
        private readonly string stopSet;

        protected YeeSupportsColorFlowMode(YeelightDevice device, string startName, string stopName,
            string flowingName, string flowParamsName) : base(device)
        {
            startSet = startName;
            stopSet = stopName;
            propFlowing = flowingName;
            propFlowParams = flowParamsName;
        }

        public YeeSupportsColorFlowMode(YeelightDevice device) :
            this(device, "start_cf", "stop_cf", "flowing", "flow_params")
        { }

        public override string[] SupportedMethods => new[] { startSet, stopSet };

        public override string[] SupportedProperties => new[] { propFlowing, propFlowParams };

        public async Task<SwitchState> GetFlowingStateAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(int)(await Device.GetPropertiesAsync(cancellationToken, propFlowing).ConfigureAwait(false))[0];
        }

        public async Task<JsonValue> GetFlowingParamsAsync(CancellationToken cancellationToken = default)
        {
            return (await Device.GetPropertiesAsync(cancellationToken, propFlowParams).ConfigureAwait(false))[0];
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
            return Device.InvokeAsync(startSet, new JsonArray(count, (int)mode, expression), cancellationToken);
        }

        public Task<JsonValue> StopColorFlowAsync(CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync(stopSet, EmptyArgs, cancellationToken);
        }
    }
}