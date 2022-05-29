using System.Diagnostics.CodeAnalysis;

using static System.Reflection.BindingFlags;

namespace IoT.Device;

[SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
public static class DeviceFactory<T>
{
    private static readonly Dictionary<string, Type> Cache = new();

#nullable enable

    public static void Register<TImpl>(string model) where TImpl : T
    {
        lock (Cache)
        {
            Cache[model] = typeof(TImpl);
        }
    }

    public static T? Create(string model, params object[] args) =>
        Cache.TryGetValue(model, out var type)
            ? (T?)Activator.CreateInstance(type, Public | NonPublic | Instance, null, args, null)
            : default;
}