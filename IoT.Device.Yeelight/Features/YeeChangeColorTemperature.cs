using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeColorTemperature : YeelightDeviceCapability
    {
        public YeeChangeColorTemperature(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();

        public Task<uint> GetColorTemperatureAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<JsonValue> SetColorTemperatureAsync(uint temperature, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}