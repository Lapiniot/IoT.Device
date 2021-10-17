namespace IoT.Device;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public abstract class ExportAttribute<T> : Attribute
{
    protected ExportAttribute(string model)
    {
        if(string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException($"'{nameof(model)}' cannot be null or whitespace.", nameof(model));
        }

        Model = model;
    }

    public string Model { get; }
}