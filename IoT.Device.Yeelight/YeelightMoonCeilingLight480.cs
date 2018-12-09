using System;
using System.Json;
using IoT.Protocol.Interfaces;

namespace IoT.Device.Yeelight
{
    public class YeelightMoonCeilingLight480 : YeelightWhiteLamp
    {
        public YeelightMoonCeilingLight480(IConnectedEndpoint<JsonObject, JsonValue> endpoint) : base(endpoint) { }

        #region Overrides of YeelightDevice

        public override string ModelName => "yeelink.light.ceiling3";

        public override string[] SupportedCapabilities => throw new NotImplementedException();

        public override string[] SupportedProperties => throw new NotImplementedException();

        public override T GetFeature<T>()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}