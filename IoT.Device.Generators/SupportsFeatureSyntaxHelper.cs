using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace IoT.Device.Generators;

internal static class SupportsFeatureSyntaxHelper
{
    public static bool IsFeatureAttribute(AttributeData attribute) => attribute is
    {
        AttributeClass:
        {
            IsGenericType: true,
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
                    }
                }
            }
        }
    };

    public static bool TryGetFeatureType(AttributeData attribute,
        [NotNullWhen(true)] out INamedTypeSymbol? featureType,
        out INamedTypeSymbol? featureImplType)
    {
        if (attribute is { AttributeClass.TypeArguments: [INamedTypeSymbol type, .. var other] })
        {
            featureType = type;
            featureImplType = other is [INamedTypeSymbol implType, ..] ? implType : null;
            return true;
        }

        featureType = null;
        featureImplType = null;
        return false;
    }
}