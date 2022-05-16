using System.Diagnostics.CodeAnalysis;

using static System.Reflection.BindingFlags;

namespace IoT.Device;

[SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
public static class DeviceFactory<T>
{
    private static readonly object syncRoot = new();
    private static readonly Dictionary<string, Type> cache = new();
    private static readonly Dictionary<Type, string> reverseCache = new();

#nullable enable

    public static void Register<TImpl>(string model) where TImpl : T
    {
        lock (syncRoot)
        {
            cache[model] = typeof(TImpl);
            reverseCache[typeof(TImpl)] = model;
        }
    }

    public static T? Create(string model, params object[] args)
    {
        return cache.TryGetValue(model, out var type)
            ? (T?)Activator.CreateInstance(type, Public | NonPublic | Instance, null, args, null)
            : default;
    }

    public static string? GetModelName<TImpl>() => reverseCache.TryGetValue(typeof(TImpl), out var model) ? model : null;
}