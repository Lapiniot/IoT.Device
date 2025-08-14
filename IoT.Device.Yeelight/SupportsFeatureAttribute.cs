namespace IoT.Device.Yeelight;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class SupportsFeatureAttribute<TFeature> :
    Device.SupportsFeatureAttribute<TFeature, TFeature>
    where TFeature : YeelightDeviceFeature
{ }

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class SupportsFeatureAttribute<TFeature, TFeatureImpl> :
    Device.SupportsFeatureAttribute<TFeature, TFeatureImpl>
    where TFeature : YeelightDeviceFeature
    where TFeatureImpl : TFeature
{ }