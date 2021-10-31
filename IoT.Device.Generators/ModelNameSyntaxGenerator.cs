using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class ModelNameSyntaxGenerator
{
    public static SyntaxNode GenerateAugmentationClass(ITypeSymbol implType, string model)
    {
        return NamespaceDeclaration(ParseName(implType.ContainingNamespace.ToDisplayString()))
            .AddMembers(
                ClassDeclaration(implType.Name)
                    .AddModifiers(Token(PublicKeyword), Token(PartialKeyword))
                    .AddMembers(
                        PropertyDeclaration(PredefinedType(Token(StringKeyword)), Identifier("ModelName"))
                            .AddModifiers(Token(PublicKeyword), Token(OverrideKeyword))
                            .AddAccessorListAccessors(
                                AccessorDeclaration(GetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SemicolonToken)))
                                    .WithInitializer(EqualsValueClause(LiteralExpression(StringLiteralExpression, Literal(model))))
                            .WithSemicolonToken(Token(SemicolonToken))))
            .NormalizeWhitespace();
    }
}