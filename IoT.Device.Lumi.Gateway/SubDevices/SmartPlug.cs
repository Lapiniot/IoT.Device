using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public class SmartPlug : LumiSubDevice
    {
        public SmartPlug(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "lumi.plug.v1";

        protected internal override void UpdateState(JsonValue jsonValue)
        {
        }
    }
}