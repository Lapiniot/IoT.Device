using System;

namespace IoT.Device
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class ExportDeviceAttributeBase : Attribute
    {
        protected ExportDeviceAttributeBase(string model, Type implementation)
        {
            Model = model;
            ImplementationType = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }

        public string Model { get; }
        public Type ImplementationType { get; }
    }
}