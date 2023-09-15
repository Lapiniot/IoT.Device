namespace IoT.Device.Metadata;

[AttributeUsage(AttributeTargets.Class)]
public sealed class PowerSourceAttribute(PowerSource source) : Attribute
{
    public PowerSource Source { get; } = source;
}