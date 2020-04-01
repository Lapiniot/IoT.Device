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

        static Container()
        {
            var baseName = typeof(Container<,>).Assembly.GetName().Name;

            var prefix = baseName + ".";

            bool Predicate(Assembly assembly)
            {
                var name = assembly.GetName().Name;

                return name.StartsWith(prefix, OrdinalIgnoreCase);
            }

            var assemblies = CurrentDomain.GetAssemblies().Where(Predicate);

            var attributes = assemblies.SelectMany(CustomAttributeExtensions.GetCustomAttributes<TAttr>).ToArray();

            Models = attributes.Where(a => !string.IsNullOrWhiteSpace(a.Model))
                .ToDictionary(a => a.Model, a => a.ImplementationType);
        }

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