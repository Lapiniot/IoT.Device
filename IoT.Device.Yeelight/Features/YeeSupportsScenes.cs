namespace IoT.Device.Yeelight.Features;

public class YeeSupportsScenes : YeelightDeviceFeature
{
    private readonly string method;

    protected YeeSupportsScenes(YeelightDevice device, string setSceneName) : base(device)
    {
        method = setSceneName;
    }

    public YeeSupportsScenes(YeelightDevice device) : this(device, "set_scene") { }

    public override IEnumerable<string> SupportedMethods => [method];

    public override IEnumerable<string> SupportedProperties => [];

    /// <summary>
    /// This method is used to set the smart LED directly to specified state. If the smart LED is off,
    /// then it will turn on the smart LED firstly and then apply the specified command.
    /// </summary>
    /// <param name="cls">
    /// Can be "color", "hsv", "ct", "cf", "auto_delay_off".
    /// <remarks>"color" means change the smart LED to specified color and brightness.</remarks>
    /// <remarks>"hsv" means change the smart LED to specified color and brightness.</remarks>
    /// <remarks>"ct" means change the smart LED to specified ct and brightness.</remarks>
    /// <remarks>"cf" means start a color flow in specified fashion.</remarks>
    /// <remarks>
    /// "auto_delay_off" means turn on the smart LED to specified brightness and
    /// start a sleep timer to turn off the light after the specified minutes.
    /// </remarks>
    /// </param>
    /// <param name="param1">Class specific param1</param>
    /// <param name="param2">Class specific param2</param>
    /// <param name="cancellationToken">Token for external cancellation</param>
    /// <returns>Operation result ("ok" or error description)</returns>
    /// <example>
    /// {"id":1, "method":"set_scene", "params":["color", 65280, 70]}
    /// {"id":1, "method":"set_scene", "params":["hsv", 300, 70, 100]}
    /// {"id":1, "method":"set_scene", "params":["ct", 5400, 100]}
    /// {"id":1, "method":"set_scene", "params":["cf", 0, 0, "500,1,255,100,1000,1,16776960,70"]}
    /// {"id":1, "method":"set_scene", "params":["auto_delay_off", 50, 5]
    /// </example>
    public Task SetSceneAsync(string cls, uint param1, uint param2, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(method, new object[] { cls, param1, param2 }, cancellationToken);

    /// <summary>
    /// This method is used to set the smart LED directly to specified state. If the smart LED is off,
    /// then it will turn on the smart LED firstly and then apply the specified command.
    /// </summary>
    /// <param name="cls">
    /// Can be "color", "hsv", "ct", "cf", "auto_delay_off".
    /// <remarks>"color" means change the smart LED to specified color and brightness.</remarks>
    /// <remarks>"hsv" means change the smart LED to specified color and brightness.</remarks>
    /// <remarks>"ct" means change the smart LED to specified ct and brightness.</remarks>
    /// <remarks>"cf" means start a color flow in specified fashion.</remarks>
    /// <remarks>
    /// "auto_delay_off" means turn on the smart LED to specified brightness and
    /// start a sleep timer to turn off the light after the specified minutes.
    /// </remarks>
    /// </param>
    /// <param name="param1">Class specific param1</param>
    /// <param name="param2">Class specific param2</param>
    /// <param name="param3">Class specific param3</param>
    /// <param name="cancellationToken">Token for external cancellation</param>
    /// <returns>Operation result ("ok" or error description)</returns>
    /// <example>
    /// {"id":1, "method":"set_scene", "params":["color", 65280, 70]}
    /// {"id":1, "method":"set_scene", "params":["hsv", 300, 70, 100]}
    /// {"id":1, "method":"set_scene", "params":["ct", 5400, 100]}
    /// {"id":1, "method":"set_scene", "params":["cf", 0, 0, "500,1,255,100,1000,1,16776960,70"]}
    /// {"id":1, "method":"set_scene", "params":["auto_delay_off", 50, 5]
    /// </example>
    public Task SetSceneAsync(string cls, uint param1, uint param2, uint param3, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(method, new object[] { cls, param1, param2, param3 }, cancellationToken);

    /// <summary>
    /// This method is used to set the smart LED directly to specified state. If the smart LED is off,
    /// then it will turn on the smart LED firstly and then apply the specified command.
    /// </summary>
    /// <param name="cls">
    /// Can be "color", "hsv", "ct", "cf", "auto_delay_off".
    /// <remarks>"color" means change the smart LED to specified color and brightness.</remarks>
    /// <remarks>"hsv" means change the smart LED to specified color and brightness.</remarks>
    /// <remarks>"ct" means change the smart LED to specified ct and brightness.</remarks>
    /// <remarks>"cf" means start a color flow in specified fashion.</remarks>
    /// <remarks>
    /// "auto_delay_off" means turn on the smart LED to specified brightness and
    /// start a sleep timer to turn off the light after the specified minutes.
    /// </remarks>
    /// </param>
    /// <param name="param1">Class specific param1</param>
    /// <param name="param2">Class specific param2</param>
    /// <param name="param3">Class specific param3</param>
    /// <param name="cancellationToken">Token for external cancellation</param>
    /// <returns>Operation result ("ok" or error description)</returns>
    /// <example>
    /// {"id":1, "method":"set_scene", "params":["color", 65280, 70]}
    /// {"id":1, "method":"set_scene", "params":["hsv", 300, 70, 100]}
    /// {"id":1, "method":"set_scene", "params":["ct", 5400, 100]}
    /// {"id":1, "method":"set_scene", "params":["cf", 0, 0, "500,1,255,100,1000,1,16776960,70"]}
    /// {"id":1, "method":"set_scene", "params":["auto_delay_off", 50, 5]
    /// </example>
    public Task SetSceneAsync(string cls, uint param1, uint param2, string param3, CancellationToken cancellationToken = default) =>
        Device.InvokeAsync(method, new object[] { cls, param1, param2, param3 }, cancellationToken);
}