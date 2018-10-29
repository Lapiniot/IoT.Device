using System;

namespace IoT.Device.Metadata
{
    public class PowerSourceAttribute : Attribute
    {
        public PowerSourceAttribute(PowerSource source)
        {
            Source = source;
        }

        public PowerSource Source { get; }
    }
}