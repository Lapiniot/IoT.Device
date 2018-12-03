using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeChangeColorHSV : YeelightDeviceCapability
    {
        public YeeChangeColorHSV(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();

        public Task<uint> GetHueAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<uint> GetSaturationAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<JsonValue> SetColorHSVAsync(uint hsv, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}