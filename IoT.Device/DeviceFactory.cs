using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using static System.Reflection.BindingFlags;

namespace IoT.Device;

[SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
public static class DeviceFactory<T>
{
    private static readonly ConcurrentDictionary<string, Type> cache = new();

#nullable enable

    public static void Register<TImpl>(string model) where TImpl : T
    {
        _ = cache.AddOrUpdate(model, typeof(TImpl), (_, _) => typeof(TImpl));
    }

    public static T? Create(string model, params object[] args)
    {
        return cache.TryGetValue(model, out var type)
            ? (T?)Activator.CreateInstance(type, Public | NonPublic | Instance, null, args, null)
            : default;
    }
}