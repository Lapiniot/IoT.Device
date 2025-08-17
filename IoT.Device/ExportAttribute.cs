namespace IoT.Device;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public abstract class ExportAttribute<T, TImpl> : Attribute
    where T : class
    where TImpl : class, T
{
    protected ExportAttribute(string modelId, string modelName)
    {
        if (string.IsNullOrWhiteSpace(modelId))
        {
            throw new ArgumentException($"'{nameof(modelId)}' cannot be null or whitespace.", nameof(modelId));
        }

        ModelId = modelId;
        ModelName = modelName;
    }

    public string ModelId { get; }

    public string ModelName { get; set; }
}