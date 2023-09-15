namespace IoT.Device.Metadata;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ModelIDAttribute(string id) : Attribute
{
    public string ID { get; } = id;
}