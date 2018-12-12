using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeProvideColorMode : YeelightDeviceFeature
    {
        public static readonly Type Type = typeof(YeeProvideColorMode);

        private readonly string property;

        protected YeeProvideColorMode(YeelightDevice device, string colorModeProp) : base(device)
        {
            property = colorModeProp;
        }

        public YeeProvideColorMode(YeelightDevice device) : this(device, "color_mode") { }

        public override string[] SupportedMethods => Array.Empty<string>();

        public override string[] SupportedProperties => new[] { property };

        public async Task<ColorMode> GetColorModeAsync(CancellationToken cancellationToken = default)
        {
            return (ColorMode)(uint)(await Device.GetPropertiesAsync(cancellationToken, property).ConfigureAwait(false))[0];
        }
    }
}