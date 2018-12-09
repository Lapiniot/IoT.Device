using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeColorMode : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeChangeColorMode);

        public YeeChangeColorMode(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => Array.Empty<string>();

        public override string[] SupportedProperties => new[] { "color_mode" };

        public async Task<ColorMode> GetColorModeAsync(CancellationToken cancellationToken = default)
        {
            return (ColorMode)(uint)(await Device.GetPropertiesAsync(cancellationToken, "color_mode").ConfigureAwait(false))[0];
        }
    }
}