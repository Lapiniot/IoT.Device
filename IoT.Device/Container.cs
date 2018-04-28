using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using static System.AppDomain;
using static System.Reflection.BindingFlags;
using static System.StringComparison;

namespace IoT.Device
{
    public static class Container<TAttr, TImpl>
        where TAttr : ExportDeviceAttributeBase
        where TImpl : class
    {
        private static readonly Dictionary<string, Type> Models;
        private static readonly Dictionary<uint, Type> Types;
        private static readonly Dictionary<string, uint> Map;

        static Container()
        {
            var baseName = typeof(Container<,>).Assembly.GetName().Name;

            var prefix = baseName + ".";

            bool Predicate(Assembly assembly)
            {
                var name = assembly.GetName().Name;

                return string.Equals(name, baseName, OrdinalIgnoreCase) ||
                       name.StartsWith(prefix, OrdinalIgnoreCase);
            }

            var assemblies = CurrentDomain.GetAssemblies().Where(Predicate);

            var attributes = assemblies.SelectMany(CustomAttributeExtensions.GetCustomAttributes<TAttr>).ToArray();

            Types = attributes.Where(a => a.DeviceType > 0).ToDictionary(a => a.DeviceType, a => a.ImplementationType);

            Models = attributes.Where(a => !string.IsNullOrWhiteSpace(a.ModelName))
                .ToDictionary(a => a.ModelName, a => a.ImplementationType);

            Map = attributes.Where(a => a.DeviceType > 0 && !string.IsNullOrWhiteSpace(a.ModelName))
                .ToDictionary(a => a.ModelName, a => a.DeviceType);
        }

        private static TImpl CreateInstance(Type type, params object[] args)
        {
            return (TImpl) Activator.CreateInstance(type, Public | NonPublic | Instance, null, args, null, null);
        }

        public static TImpl CreateInstance(uint deviceType, params object[] args)
        {
            return Types.TryGetValue(deviceType, out var type) ? CreateInstance(type, args) : null;
        }

        public static TImpl CreateInstance(string deviceModel, params object[] args)
        {
            return Models.TryGetValue(deviceModel, out var type) ? CreateInstance(type, args) : null;
        }

        public static bool TryGetType(string model, out uint deviceType)
        {
            return Map.TryGetValue(model, out deviceType);
        }

        public static bool TryGetType<T>(out uint deviceType) where T : TImpl
        {
            var pair = Types.FirstOrDefault(t => t.Value == typeof(T));

            deviceType = pair.Key;

            return pair.Value != null;
        }
    }
}