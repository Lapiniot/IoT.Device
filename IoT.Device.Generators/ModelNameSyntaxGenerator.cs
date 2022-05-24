using Microsoft.CodeAnalysis;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class ModelNameSyntaxGenerator
{
    public static SyntaxNode GenerateAugmentationClass(string className, string namespaceName, string model) =>
        NamespaceDeclaration(ParseName(namespaceName))
            .AddMembers(
                ClassDeclaration(className)
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