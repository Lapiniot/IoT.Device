﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeSupportsColorFlowMode : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeSupportsColorFlowMode);

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

        public override IEnumerable<string> SupportedMethods => new[] {startSet, stopSet};

        public override IEnumerable<string> SupportedProperties => new[] {propFlowing, propFlowParams};

        public async Task<SwitchState> GetFlowingStateAsync(CancellationToken cancellationToken = default)
        {
            return (SwitchState)(await Device.GetPropertyAsync(propFlowing, cancellationToken).ConfigureAwait(false)).GetInt32();
        }

        public Task<JsonElement> GetFlowingParamsAsync(CancellationToken cancellationToken = default)
        {
            return Device.GetPropertyAsync(propFlowParams, cancellationToken);
        }

        public Task StartColorFlowAsync(uint count, PostFlowAction mode,
            (int Duration, FlowTransition Mode, int Value, int Brightness)[] states,
            CancellationToken cancellationToken)
        {
            if(states is null) throw new ArgumentNullException(nameof(states));

            var sb = new StringBuilder();

            foreach(var (duration, flowTransition, value, brightness) in states)
            {
                sb.Append(duration);
                sb.Append(',');
                sb.Append((int)flowTransition);
                sb.Append(',');
                sb.Append(value);
                sb.Append(',');
                sb.Append(brightness);
                sb.Append(',');
            }

            if(sb[^1] == ',') sb.Length--;

            return StartColorFlowAsync(count, mode, sb.ToString(), cancellationToken);
        }

        public Task StartColorFlowAsync(uint count = 0, PostFlowAction mode = PostFlowAction.RestoreState,
            string expression = "", CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync(startSet, new object[] { count, (int)mode, expression }, cancellationToken);
        }

        public Task StopColorFlowAsync(CancellationToken cancellationToken = default)
        {
            return Device.InvokeAsync(stopSet, EmptyArgs, cancellationToken);
        }
    }
}