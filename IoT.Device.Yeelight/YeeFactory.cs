using IoT.Protocol.Yeelight;

namespace IoT.Device.Yeelight
{
    public static class YeeFactory
    {
        public static YeelightDevice Create(string model, YeelightControlEndpoint endpoint)
        {
            switch (model)
            {
                case "yeelink.light.color2": return new YeelightColorBulb2(endpoint);
                case "yeelink.light.strip2": return new YeelightColorStripPlus(endpoint);
                case "yeelink.light.ceiling3": return new YeelightMoonCeilingLight480(endpoint);
                case "yeelink.light.ceiling4": return new YeelightMoonCeilingLight650(endpoint);
                default:
                    return null;
            }
        }
    }
}