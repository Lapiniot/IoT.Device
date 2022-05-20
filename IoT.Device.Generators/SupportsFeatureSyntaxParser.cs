using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class SupportsFeatureSyntaxParser
{
    public static bool IsSuitableCandidate(SyntaxNode node) =>
        node is ClassDeclarationSyntax { Modifiers: var tokenList } &&
        tokenList.Any(token => token.IsKind(PublicKeyword)) &&
        tokenList.Any(token => token.IsKind(PartialKeyword)) &&
        !tokenList.Any(token => token.IsKind(StaticKeyword));

    public static ITypeSymbol? Parse(ClassDeclarationSyntax node, SemanticModel model, CancellationToken cancellationToken)
    {
        return model.GetDeclaredSymbol(node, cancellationToken) is ITypeSymbol symbol ? symbol : null;
    }
}