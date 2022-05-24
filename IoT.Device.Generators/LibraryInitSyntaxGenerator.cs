using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class LibraryInitSyntaxGenerator
{
    public static SyntaxNode GenerateLibInitClass(string namespaceName, string className, string initMethodName,
        IEnumerable<(string TargetType, string ImplType, string Model)> exports)
    {
        return NamespaceDeclaration(ParseName(namespaceName))
            .AddUsings(UsingDirective(ParseName("IoT.Device")))
            .AddMembers(ClassDeclaration(className)
                .AddModifiers(Token(PublicKeyword), Token(StaticKeyword), Token(PartialKeyword))
                .AddMembers(
                    MethodDeclaration(PredefinedType(Token(VoidKeyword)), initMethodName)
                        .AddModifiers(Token(PublicKeyword), Token(StaticKeyword))
                        .WithBody(Block(GenerateExportStatements(exports).Append(
                            ExpressionStatement(InvocationExpression(IdentifierName($"{initMethodName}Extra")))))),
                    MethodDeclaration(PredefinedType(Token(VoidKeyword)), $"{initMethodName}Extra")
                        .AddModifiers(Token(StaticKeyword), Token(PartialKeyword))
                        .WithSemicolonToken(Token(SemicolonToken))))
            .NormalizeWhitespace();
    }

    private static IEnumerable<StatementSyntax> GenerateExportStatements(
        IEnumerable<(string TargetType, string ImplType, string Model)> exports) =>
        exports.Select(d => ExpressionStatement(
            InvocationExpression(
                MemberAccessExpression(SimpleMemberAccessExpression,
                    GenericName(Identifier("DeviceFactory"), TypeArgumentList(SingletonSeparatedList(ParseTypeName(d.TargetType)))),
                    GenericName(Identifier("Register"), TypeArgumentList(SingletonSeparatedList(ParseTypeName(d.ImplType))))),
                ArgumentList(SingletonSeparatedList(Argument(LiteralExpression(StringLiteralExpression,
                    Literal(d.Model))))))));
}