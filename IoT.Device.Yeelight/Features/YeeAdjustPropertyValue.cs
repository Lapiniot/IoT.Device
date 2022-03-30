namespace IoT.Device.Yeelight.Features;

public abstract class YeeAdjustPropertyValue : YeelightDeviceFeature
{
    private readonly string method;

    protected YeeAdjustPropertyValue(YeelightDevice device, string adjustMethodName) : base(device)
    {
        method = adjustMethodName;
    }

    public override IEnumerable<string> SupportedMethods => new[] { method };
    public override IEnumerable<string> SupportedProperties => Array.Empty<string>();

    /// <summary>
    /// This method is used to adjust the property value by specified percentage within specified duration.
    /// </summary>
    /// <param name="percentage">Percentage to be adjusted. The range is: -100 ~ 100</param>
    /// <param name="durationMilliseconds">
    /// Specifies the total time of the gradual change. The unit is milliseconds. The
    /// minimum supported duration is 30 milliseconds.
    /// </param>
    /// <param name="cancellationToken">Token for external cancellation</param>
    /// <returns>Operation result ("ok" or error description)</returns>
    public Task AdjustValueAsync(int percentage, uint durationMilliseconds, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(method, new[] { percentage, (int)durationMilliseconds }, cancellationToken);
}