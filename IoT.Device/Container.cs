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
        private static readonly string Prefix = $"{typeof(Container<,>).Assembly.GetName().Name}.";

        private static readonly Dictionary<string, Type> Models = CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name.StartsWith(Prefix, OrdinalIgnoreCase))
            .SelectMany(CustomAttributeExtensions.GetCustomAttributes<TAttr>)
            .Where(a => !string.IsNullOrWhiteSpace(a.Model))
            .ToDictionary(a => a.Model, a => a.ImplementationType);

        private static TImpl CreateInstance(Type type, params object[] args)
        {
            return (TImpl)Activator.CreateInstance(type, Public | NonPublic | Instance, null, args, null, null);
        }

        [SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
        public static TImpl CreateInstance(string deviceModel, params object[] args)
        {
            var v = Models.TryGetValue(deviceModel, out var type);
            return v ? CreateInstance(type, args) : null;
        }
    }
}