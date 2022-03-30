namespace IoT.Device.Yeelight.Features;

public class YeeSupportsAmbientColorFlowMode : YeeSupportsColorFlowMode
{
    public static new readonly Type Type = typeof(YeeSupportsAmbientColorFlowMode);

    public YeeSupportsAmbientColorFlowMode(YeelightDevice device) :
        base(device, "bg_start_cf", "bg_stop_cf", "bg_flowing", "bg_flow_params")
    { }
}