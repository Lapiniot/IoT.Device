using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeColorRGB : YeelightDeviceCapability
    {
        public YeeChangeColorRGB(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();

        public Task<uint> GetColorRGBAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<JsonValue> SetColorRGBAsync(uint rgb, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}