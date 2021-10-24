using Microsoft.CodeAnalysis;

namespace IoT.Device.Generators;

internal static class SemanticModelExtensions
{
    public static IEnumerable<ITypeSymbol> EnumerateRelatedFeatureTypes(this ITypeSymbol? symbol)
    {
        while(symbol is { } and not
            {
                BaseType:
                {
                    Name: "DeviceFeature",
                    ContainingAssembly.Name: "IoT.Device",
                    ContainingNamespace:
                    {
                        Name: "Device",
                        ContainingNamespace:
                        {
                            Name: "IoT",
                            ContainingNamespace.IsGlobalNamespace: true
                        },
                    }
                }
            })
        {
            yield return symbol;
            symbol = symbol.BaseType;
        }
    }

    public static bool IsSupportsFeatureAtribute(this ITypeSymbol? symbol)
    {
        return symbol is INamedTypeSymbol
        {
            IsGenericType: true,
            TypeArguments.Length: 1,
            BaseType:
            {
                Name: "SupportsFeatureAttribute",
                ContainingAssembly.Name: "IoT.Device",
                ContainingNamespace:
                {
                    Name: "Device",
                    ContainingNamespace:
                    {
                        Name: "IoT",
                        ContainingNamespace.IsGlobalNamespace: true
                    },
                }
            }
        };
    }

    public static bool HasOverrideForGetFeatureMethod(this ITypeSymbol? symbol)
    {
        while(symbol is { })
        {
            if(symbol.GetMembers("GetFeature").OfType<IMethodSymbol>().Any(m => m is
                {
                    IsGenericMethod: true,
                    TypeArguments.Length: 1,
                    Parameters.Length: 0,
                    IsOverride: true
                }) || symbol.GetAttributes().Any(a => a.AttributeClass.IsSupportsFeatureAtribute()))
            {
                return true;
            }
            symbol = symbol.BaseType;
        }
        return false;
    }
}