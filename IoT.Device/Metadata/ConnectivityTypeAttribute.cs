namespace IoT.Device.Metadata;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ConnectivityTypeAttribute : Attribute
{
    public ConnectivityTypeAttribute(ConnectivityTypes connectivity) => Connectivity = connectivity;

    public ConnectivityTypes Connectivity { get; }
}