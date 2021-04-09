using System;

namespace IoT.Device.Metadata
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConnectivityTypeAttribute : Attribute
    {
        public ConnectivityTypeAttribute(ConnectivityTypes type)
        {
            Connectivity = type;
        }

        public ConnectivityTypes Connectivity { get; }
    }
}