using Microsoft.CodeAnalysis;

namespace IoT.Device.Generators.Helpers;

internal static class SemanticModelExtensions
{
    public static IEnumerable<ITypeSymbol> EnumerateRelatedFeatureTypes(this ITypeSymbol? symbol, bool includeInterfaces = true)
    {
        while (symbol is { } and not
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
            if (includeInterfaces)
            {
                foreach (var iface in symbol.Interfaces)
                {
                    yield return iface;
                }
            }

            symbol = symbol.BaseType;
        }
    }

    /// <summary>
    /// Checks whether supplied symbol designates known generic SupportsFeatureAttribute
    /// with either 1 or 2 generic parameters
    /// </summary>
    /// <param name="symbol">Type symbol to check</param>
    /// <returns><value>true</value> when matches, otherwise <value>false</value></returns>
    public static bool IsSupportsFeatureAttribute(this ITypeSymbol? symbol)
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

    public static INamedTypeSymbol[] GetSupportsFeatureAttributes(this ITypeSymbol symbol) =>
        symbol.GetAttributes()
            .Where(a => a.AttributeClass.IsSupportsFeatureAttribute()).Select(a => a.AttributeClass!)
            .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default).ToArray();

    public static bool HasOverrideForGetFeatureMethod(this ITypeSymbol? symbol)
    {
        while (symbol is { })
        {
            if (symbol.GetMembers("GetFeature").OfType<IMethodSymbol>().Any(m => m is
                {
                    IsGenericMethod: true,
                    TypeArguments.Length: 1,
                    Parameters.Length: 0,
                    IsOverride: true
                }) || symbol.GetAttributes().Any(a => a.AttributeClass.IsSupportsFeatureAttribute()))
            {
                return true;
            }

            symbol = symbol.BaseType;
        }

        return false;
    }

    public static IEnumerable<ITypeSymbol> GetBaseTypes(this ITypeSymbol? symbol)
    {
        while ((symbol = symbol?.BaseType) is { })
        {
            yield return symbol;
        }
    }
}