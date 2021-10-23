namespace IoT.Device.Yeelight;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class SupportsFeatureAttribute<TFeature> : SupportsFeatureAttribute where TFeature : YeelightDeviceFeature
{ }

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class SupportsFeatureAttribute<TFeature, TImpl> : SupportsFeatureAttribute
    where TFeature : YeelightDeviceFeature
    where TImpl : TFeature
{ }