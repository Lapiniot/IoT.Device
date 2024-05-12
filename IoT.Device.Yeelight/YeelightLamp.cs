namespace IoT.Device.Yeelight;

[SupportsFeature<ChangePowerState>, SupportsFeature<ChangeDeviceName>, SupportsFeature<AdjustProperty>,
SupportsFeature<SupportsSaveState>, SupportsFeature<SupportsCronScheduler>, SupportsFeature<SupportsScenes>]
public abstract partial class YeelightLamp(YeelightControlEndpoint endpoint) : YeelightDevice(endpoint) { }