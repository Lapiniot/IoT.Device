using System;

namespace IoT.Device
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class ExportDeviceAttributeBase : Attribute
    {
        protected ExportDeviceAttributeBase(string model, Type implementationType)
        {
            Model = model;
            ImplementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
        }

        public string Model { get; }
        public Type ImplementationType { get; }
    }
}