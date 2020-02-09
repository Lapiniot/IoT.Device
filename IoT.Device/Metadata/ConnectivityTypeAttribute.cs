using System;

namespace IoT.Device.Metadata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConnectivityTypeAttribute : Attribute
    {
        public ConnectivityTypeAttribute(ConnectivityTypes type)
        {
            Connectivity = type;
        }

        public ConnectivityTypes Connectivity { get; }
    }
}