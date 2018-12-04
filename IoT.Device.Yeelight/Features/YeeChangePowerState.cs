using System;
using System.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Device.Yeelight.Features
{
    public sealed class YeeChangePowerState : YeelightDeviceFeature
    {
        public YeeChangePowerState(YeelightDevice device) : base(device) { }

        public override string[] SupportedMethods => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();

        public Task<SwitchState> GetPowerStateAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<JsonValue> SetPowerStateAsync(SwitchState state = SwitchState.On, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}