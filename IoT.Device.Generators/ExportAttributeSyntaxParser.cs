using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IoT.Device.Generators;

public static class ExportAttributeSyntaxParser
{
    private const string ExportAttributeName = "ExportAttribute";
    private const string AssemblyName = "IoT.Device";
    private const string DeviceNsName = "Device";
    private const string IoTNsName = "IoT";

    public static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode) =>
       syntaxNode is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    public static ClassDeclarationSyntax? GetSemanticTargetForGeneration(ClassDeclarationSyntax syntax,
        SemanticModel semanticModel, CancellationToken cancellationToken)
    {
        foreach (var attrList in syntax.AttributeLists)
        {
            foreach (var attribute in attrList.Attributes)
            {
                if (semanticModel.GetSymbolInfo(attribute, cancellationToken).Symbol is IMethodSymbol
                    {
                        ContainingType:
                        {
                            IsGenericType: false,
                            BaseType:
                            {
                                IsGenericType: true,
                                Name: ExportAttributeName,
                                ContainingAssembly.Name: AssemblyName,
                                ContainingNamespace: { Name: DeviceNsName, ContainingNamespace: { Name: IoTNsName, ContainingNamespace.IsGlobalNamespace: true } },
                                TypeArguments.Length: 2
                            }
                        },
                        Parameters: [{ Type.SpecialType: SpecialType.System_String }, ..]
                    })
                {
                    return syntax;
                }
            }
        }

        return null;
    }

    public static bool TryGetExportAttribute(INamedTypeSymbol symbol, out AttributeData? attributeData, out ITypeSymbol? targetType)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (attribute is
                {
                    AttributeClass:
                    {
                        IsGenericType: false,
                        BaseType:
                        {
                            IsGenericType: true,
                            Name: ExportAttributeName,
                            ContainingAssembly.Name: AssemblyName,
                            ContainingNamespace: { Name: DeviceNsName, ContainingNamespace: { Name: IoTNsName, ContainingNamespace.IsGlobalNamespace: true } },
                            TypeArguments: [var typeArg, ..]
                        }
                    }
                })
            {
                targetType = typeArg;
                attributeData = attribute;
                return true;
            }
        }

        attributeData = null;
        targetType = null;
        return false;
    }
}