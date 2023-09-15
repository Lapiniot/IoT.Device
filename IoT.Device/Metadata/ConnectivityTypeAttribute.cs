namespace IoT.Device.Metadata;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ConnectivityTypeAttribute(ConnectivityTypes connectivity) : Attribute
{
    public ConnectivityTypes Connectivity { get; } = connectivity;
}