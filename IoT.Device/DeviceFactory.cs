using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;
using static System.Reflection.BindingFlags;

namespace IoT.Device;

public static class DeviceFactory<T>
{
    private static readonly ConcurrentDictionary<string, Type> Cache = new(StringComparer.Ordinal);
#nullable enable

#pragma warning disable CA1000 // Do not declare static members on generic types
    public static void Register<[DynamicallyAccessedMembers(PublicConstructors | NonPublicConstructors)] TImpl>(string model) where TImpl : T =>
        Cache.AddOrUpdate(model, static (_, type) => type, static (_, _, type) => type, typeof(TImpl));

    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2067:DynamicallyAccessedMembers", Justification = "Preserved by annotating Register<TImpl>.")]
    public static T? Create(string model, params object[] args) =>
        Cache.TryGetValue(model, out var type)
            ? (T?)Activator.CreateInstance(type, Public | NonPublic | Instance, null, args, null)
            : default;
#pragma warning restore CA1000 // Do not declare static members on generic types
}