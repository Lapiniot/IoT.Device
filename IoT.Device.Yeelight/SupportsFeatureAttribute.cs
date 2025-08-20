using IoT.Device.Yeelight;

namespace IoT.Device.Generators;

internal sealed partial class SupportsFeatureAttribute<TFeature>
    where TFeature : YeelightDeviceFeature
{ }

internal sealed partial class SupportsFeatureAttribute<TFeature, TFeatureImpl>
    where TFeature : YeelightDeviceFeature
    where TFeatureImpl : TFeature
{ }