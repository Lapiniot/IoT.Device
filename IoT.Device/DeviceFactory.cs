using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using static System.AppDomain;
using static System.Reflection.BindingFlags;
using static System.StringComparison;

namespace IoT.Device;

public static class DeviceFactory<T>
{
    private static readonly Dictionary<string, Type> cache = BuildCache();

    private static Dictionary<string, Type> BuildCache()
    {
        var prefix = $"{typeof(DeviceFactory<>).Assembly.GetName().Name}.";
        var targetType = typeof(T);
        var exportAttributeType = typeof(ExportAttribute<>);

        return CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith(prefix, OrdinalIgnoreCase))
                .SelectMany(a => a.GetCustomAttributes().Where(attr => Matches(attr.GetType())))
                .ToDictionary(
                    a => (string)a.GetType().GetProperty(nameof(ExportAttribute<T>.Model)).GetValue(a),
                    a => a.GetType().BaseType.GetGenericArguments()[0]);

        bool Matches(Type type)
        {
            return type.IsGenericType && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == exportAttributeType &&
                HasConstraintOfType(type.GetGenericTypeDefinition().BaseType.GetGenericArguments()[0], targetType);
        }
    }

    private static bool HasConstraintOfType(Type genericArgumentType, Type constraintType)
    {
        return genericArgumentType.GetGenericParameterConstraints().Any(type => type == constraintType);
    }

    [SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
    public static T Create(string model, params object[] args)
    {
        return cache.TryGetValue(model, out var type)
            ? (T)Activator.CreateInstance(type, Public | NonPublic | Instance, null, args, null, null)
            : default;
    }
}