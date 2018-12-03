using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeAmbientBrightness : YeelightDeviceCapability
    {
        public YeeChangeAmbientBrightness(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();

        public Task<uint> GetBrightnessAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<JsonValue> SetBrightnessAsync(uint brightness, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}