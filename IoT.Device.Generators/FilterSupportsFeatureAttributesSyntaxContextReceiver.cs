using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal class FilterSupportsFeatureAttributesSyntaxContextReceiver : ISyntaxContextReceiver
{
    public List<ITypeSymbol> Candidates { get; } = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if(context.Node is ClassDeclarationSyntax cd &&
            cd.Modifiers.Any(m => m.IsKind(PublicKeyword)) &&
            cd.Modifiers.Any(m => m.IsKind(PartialKeyword)) &&
            !cd.Modifiers.Any(m => m.IsKind(StaticKeyword)) &&
            context.SemanticModel.GetDeclaredSymbol(context.Node) is ITypeSymbol symbol)
        {
            Candidates.Add(symbol);
        }
    }

    private static bool IsSupportsFeatureAtribute(AttributeData attribute)
    {
        return attribute is
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
                        Name: "Device", ContainingNamespace:
                        {
                            Name: "IoT", ContainingNamespace.IsGlobalNamespace: true
                        }
                    }
                },
                TypeArguments.Length: 1
            }
        };
    }

    public static IEnumerable<ITypeSymbol> EnumerateRelatedFeatureTypes(ITypeSymbol? featureType)
    {
        while(featureType is { } and not
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
                    }
                }
            })
        {
            yield return featureType;
            featureType = featureType.BaseType;
        }
    }
}