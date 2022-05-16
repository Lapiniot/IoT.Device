namespace IoT.Device;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public abstract class ExportAttribute<T, TImpl> : Attribute
    where T : class
    where TImpl : class, T
{
    protected ExportAttribute(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException($"'{nameof(model)}' cannot be null or whitespace.", nameof(model));
        }

        Model = model;
    }

    public string Model { get; }
}