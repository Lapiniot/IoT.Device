namespace IoT.Device.Yeelight
{
    public enum LightColorMode
    {
        Normal = 0, //Turn on operation (default value)
        ColorTemperature = 1, //Turn on and switch to CT mode.
        Rgb = 2, //Turn on and switch to RGB mode.
        Hsv = 3, //Turn on and switch to HSV mode
        Flow = 4, //Turn on and switch to color flow mode
        MoonLight = 5 //Turn on and switch to Night light mode. (Ceiling light only)
    }

    public enum SwitchState
    {
        Off = 0,
        On = 1,
        Offing,
        Oning
    }

    public enum Effect
    {
        Smooth,
        Sudden
    }
}