using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight;

public static class YeeFactory
{
    public static YeelightDevice Create(string model, YeelightControlEndpoint endpoint)
    {
        return model switch
        {
            "yeelink.light.color2" => new YeelightColorBulb2(endpoint),
            "yeelink.light.strip2" => new YeelightColorStripPlus(endpoint),
            "yeelink.light.ceiling3" => new YeelightMoonCeilingLight480(endpoint),
            "yeelink.light.ceiling4" => new YeelightMoonCeilingLight650(endpoint),
            _ => null
        };
    }
}