using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class SupportsFeatureSyntaxParser
{
    public static bool IsSuitableCandidate(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax { Modifiers: { } m } &&
            m.Any(m => m.IsKind(PublicKeyword)) &&
            m.Any(m => m.IsKind(PartialKeyword)) &&
            !m.Any(m => m.IsKind(StaticKeyword));
    }

    public static ITypeSymbol? Parse(ClassDeclarationSyntax node, SemanticModel model, CancellationToken cancellationToken)
    {
        return model.GetDeclaredSymbol(node, cancellationToken) is ITypeSymbol symbol ? symbol : null;
    }
}