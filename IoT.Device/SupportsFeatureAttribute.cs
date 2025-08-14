namespace IoT.Device;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class SupportsFeatureAttribute<TFeature, TFeatureImpl> : Attribute
    where TFeatureImpl : TFeature
{ }