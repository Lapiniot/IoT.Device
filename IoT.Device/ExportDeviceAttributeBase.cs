namespace IoT.Device;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public abstract class ExportDeviceAttributeBase : Attribute
{
    protected ExportDeviceAttributeBase(string model, Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(implementationType);

        Model = model;
        ImplementationType = implementationType;
    }

    public string Model { get; }
    public Type ImplementationType { get; }
}