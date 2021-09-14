namespace IoT.Device.Metadata;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ModelIDAttribute : Attribute
{
    public ModelIDAttribute(string id)
    {
        ID = id;
    }

    public string ID { get; }
}