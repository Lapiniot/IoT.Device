namespace IoT.Device.Yeelight;

[SupportsFeature<ChangeColorTemperature>, SupportsFeature<ChangeColorRGB>, SupportsFeature<ChangeColorHSV>,
SupportsFeature<AdjustColor>, SupportsFeature<AdjustColorTemperature>]
public abstract partial class YeelightColorLamp(YeelightControlEndpoint endpoint) : YeelightWhiteLamp(endpoint) { }