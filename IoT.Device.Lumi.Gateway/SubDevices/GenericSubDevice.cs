using System.Json;

namespace IoT.Device.Lumi.Gateway.SubDevices
{
    public class GenericSubDevice : LumiSubDevice
    {
        public GenericSubDevice(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName => "generic.unknown";

        protected internal override void UpdateState(JsonObject data)
        {
            if (data.TryGetValue("voltage", out var v)) Voltage = new decimal(v, 0, 0, false, 3);
        }
    }
}