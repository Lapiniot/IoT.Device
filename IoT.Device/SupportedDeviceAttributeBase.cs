using System;

namespace IoT.Device
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class SupportedDeviceAttributeBase : Attribute
    {
        protected SupportedDeviceAttributeBase(uint deviceType, Type implementation, string modelName)
        {
            ModelName = modelName;
            DeviceType = deviceType;
            ImplementationType = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }

        public string ModelName { get; }
        public uint DeviceType { get; }
        public Type ImplementationType { get; }
    }
}