namespace IoT.Device.Metadata;

[AttributeUsage(AttributeTargets.Class)]
public sealed class PowerSourceAttribute : Attribute
{
    public PowerSourceAttribute(PowerSource source)
    {
        Source = source;
    }

    public PowerSource Source { get; }
}