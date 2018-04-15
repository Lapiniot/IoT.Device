using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.StringComparison;

namespace IoT.Device
{
    public static class ImplementationCache<TAttr, TImpl>
        where TAttr : SupportedDeviceAttributeBase
        where TImpl : class
    {
        private static readonly Dictionary<string, Type> models;
        private static readonly Dictionary<uint, Type> types;
        private static readonly Dictionary<string, uint> map;

        static ImplementationCache()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().
                Where(a => a.GetName().Name.StartsWith("IoT.Device.", OrdinalIgnoreCase));

            var attributes = assemblies.
                SelectMany(CustomAttributeExtensions.GetCustomAttributes<TAttr>).
                ToArray();

            types = attributes.Where(a => a.DeviceType > 0).ToDictionary(a => a.DeviceType, a => a.ImplementationType);

            models = attributes.Where(a => !string.IsNullOrWhiteSpace(a.ModelName)).
                ToDictionary(a => a.ModelName, a => a.ImplementationType);

            map = attributes.Where(a => a.DeviceType > 0 && !string.IsNullOrWhiteSpace(a.ModelName)).
                ToDictionary(a => a.ModelName, a => a.DeviceType);
        }

        public static TImpl CreateInstance(uint deviceType, params object[] args)
        {
            return types.TryGetValue(deviceType, out var type) ? (TImpl)Activator.CreateInstance(type, args) : null;
        }

        public static TImpl CreateInstance(string deviceModel, params object[] args)
        {
            return models.TryGetValue(deviceModel, out var type) ? (TImpl)Activator.CreateInstance(type, args) : null;
        }

        public static bool TryGetType(string model, out uint deviceType)
        {
            return map.TryGetValue(model, out deviceType);
        }

        public static bool TryGetType<T>(out uint deviceType) where T : TImpl
        {
            var pair = types.FirstOrDefault(t => t.Value == typeof(T));

            deviceType = pair.Key;

            return pair.Value != null;
        }
    }
}