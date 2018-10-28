namespace IoT.Device.Lumi.SubDevices
{
    public sealed class GenericSubDevice : LumiSubDevice
    {
        internal GenericSubDevice(string sid, int id) : base(sid, id)
        {
        }

        public override string ModelName { get; } = "generic.unknown";
    }
}