using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IoT.Device.Generators;

internal static class SupportsFeatureSyntaxParser
{
    public static bool IsSyntaxTargetForGeneration(SyntaxNode syntaxNode) =>
        syntaxNode is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    public static ClassDeclarationSyntax? GetSemanticTargetForGeneration(ClassDeclarationSyntax syntax, SemanticModel model, CancellationToken cancellationToken)
    {
        foreach (var attrList in syntax.AttributeLists)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var attribute in attrList.Attributes)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (model.GetSymbolInfo(attribute, cancellationToken).Symbol is IMethodSymbol
                    {
                        ContainingType:
                        {
                            IsGenericType: true,
                            TypeArguments.Length: 1 or 2,
                            BaseType:
                            {
                                Name: "SupportsFeatureAttribute",
                                ContainingAssembly.Name: "IoT.Device",
                                ContainingNamespace: { Name: "Device", ContainingNamespace: { Name: "IoT", ContainingNamespace.IsGlobalNamespace: true } }
                            }
                        }
                    })
                {
                    return syntax;
                }
            }
        }

        return null;
    }
}