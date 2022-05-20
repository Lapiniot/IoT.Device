using System.Globalization;
using System.Text;
using System.Text.Json;

namespace IoT.Device.Yeelight.Features;

public class YeeSupportsColorFlowMode : YeelightDeviceFeature
{
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

    public override IEnumerable<string> SupportedMethods => new[] { startSet, stopSet };

    public override IEnumerable<string> SupportedProperties => new[] { propFlowing, propFlowParams };

    public async Task<SwitchState> GetFlowingStateAsync(CancellationToken cancellationToken = default) =>
        (SwitchState)int.Parse(
            (await Device.GetPropertyAsync(propFlowing, cancellationToken).ConfigureAwait(false)).GetString()!,
            CultureInfo.InvariantCulture);

    public Task<JsonElement> GetFlowingParamsAsync(CancellationToken cancellationToken = default) =>
        Device.GetPropertyAsync(propFlowParams, cancellationToken);

    public Task StartColorFlowAsync(uint count, PostFlowAction mode,
        (int Duration, FlowTransition Mode, int Value, int Brightness)[] states,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(states);

        var sb = new StringBuilder();

        foreach (var (duration, flowTransition, value, brightness) in states)
        {
            _ = sb.Append(duration);
            _ = sb.Append(',');
            _ = sb.Append((int)flowTransition);
            _ = sb.Append(',');
            _ = sb.Append(value);
            _ = sb.Append(',');
            _ = sb.Append(brightness);
            _ = sb.Append(',');
        }

        if (sb[^1] == ',') sb.Length--;

        return StartColorFlowAsync(count, mode, sb.ToString(), cancellationToken);
    }

    public Task StartColorFlowAsync(uint count = 0, PostFlowAction mode = PostFlowAction.RestoreState, string expression = "", CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(startSet, new object[] { count, (int)mode, expression }, cancellationToken);

    public Task StopColorFlowAsync(CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(stopSet, Array.Empty<object>(), cancellationToken);
}