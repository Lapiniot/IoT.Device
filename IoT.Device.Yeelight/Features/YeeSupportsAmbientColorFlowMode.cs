namespace IoT.Device.Yeelight.Features;

public class YeeSupportsAmbientColorFlowMode(YeelightDevice device) : YeeSupportsColorFlowMode(device, "bg_start_cf", "bg_stop_cf", "bg_flowing", "bg_flow_params")
{
}