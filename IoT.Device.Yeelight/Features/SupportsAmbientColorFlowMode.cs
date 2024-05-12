namespace IoT.Device.Yeelight.Features;

public class SupportsAmbientColorFlowMode(YeelightDevice device) : SupportsColorFlowMode(device, "bg_start_cf", "bg_stop_cf", "bg_flowing", "bg_flow_params") { }