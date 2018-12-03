using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public class YeeSupportsLANControl : YeelightDeviceCapability
    {
        public YeeSupportsLANControl(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();

        public Task<SwitchState> GetLANControlModeAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<JsonValue> SetLANControlModeAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}