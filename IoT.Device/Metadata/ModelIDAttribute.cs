using System;

namespace IoT.Device.Metadata
{
    public class ModelIDAttribute : Attribute
    {
        public ModelIDAttribute(string id)
        {
            ID = id;
        }

        public string ID { get; }
    }
}