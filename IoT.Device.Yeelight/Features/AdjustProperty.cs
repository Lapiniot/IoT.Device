namespace IoT.Device.Yeelight.Features;

public class AdjustProperty : YeelightDeviceFeature
{
    private readonly string method;

    protected AdjustProperty(YeelightDevice device, string adjustMethodName) : base(device) => method = adjustMethodName;

    public AdjustProperty(YeelightDevice device) : this(device, "set_adjust") { }

    public override IEnumerable<string> SupportedMethods => [method];

    public override IEnumerable<string> SupportedProperties => [];

    /// <summary>
    /// This method is used to change brightness, CT or color of a smart LED without knowing the current value,
    /// it's mainly used by controllers.
    /// </summary>
    /// <param name="action">The direction of the adjustment</param>
    /// <param name="propName">
    /// The property to adjust.
    /// <remarks>
    /// The valid value can be:
    /// “bright":   adjust brightness,
    /// “ct":       adjust color temperature,
    /// “color":    adjust color.
    /// When <paramref name="propName" /> is “color", the <paramref name="action" /> can only
    /// be <value>AdjustDirection.Circle</value>, otherwise, it will be deemed as invalid request.)
    /// </remarks>
    /// </param>
    /// <param name="cancellationToken">Token for external cancellation.</param>
    /// <returns>Operation result ("ok" or error description)</returns>
    public Task SetAdjustAsync(AdjustDirection action, string propName, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(method, new object[] { action.ToString().ToLowerInvariant(), propName }, cancellationToken);
}