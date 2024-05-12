namespace IoT.Device.Yeelight;

[SupportsFeature<ChangeBrightness>, SupportsFeature<AdjustBrightness>,
SupportsFeature<SupportsColorFlowMode>, SupportsFeature<ProvideColorMode>]
public abstract partial class YeelightWhiteLamp(YeelightControlEndpoint endpoint) : YeelightLamp(endpoint) { }