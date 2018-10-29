using System;

namespace IoT.Device.Metadata
{
    public class ConnectivityAttribute : Attribute
    {
        public ConnectivityAttribute(Connectivity type)
        {
            Type = type;
        }

        public Connectivity Type { get; }
    }
}