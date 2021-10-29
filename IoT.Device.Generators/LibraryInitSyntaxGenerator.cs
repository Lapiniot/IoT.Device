using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class LibraryInitSyntaxGenerator
{
    public static SyntaxNode GenerateLibInitClass(string namespaceName,
        string className, string initMethodName,
        List<(ITypeSymbol Type, ITypeSymbol ImplType, string Model)> exports)
    {
        var reduced = NameHelper.ExtractNames(exports, out var namespaces);

        return NamespaceDeclaration(ParseName(namespaceName))
            .AddUsings(UsingDirective(ParseName("System.Runtime.CompilerServices")), UsingDirective(ParseName("IoT.Device")))
            .AddUsings(namespaces.OrderBy(n => n).Select(ns => UsingDirective(ParseName(ns))).ToArray())
            .AddMembers(ClassDeclaration(className)
                .AddModifiers(Token(PublicKeyword), Token(StaticKeyword))
                .AddMembers(
                    MethodDeclaration(PredefinedType(Token(VoidKeyword)), initMethodName)
                        .AddModifiers(Token(PublicKeyword), Token(StaticKeyword))
                        .WithBody(Block(ParseStatement(string.Empty))),
                    MethodDeclaration(PredefinedType(Token(VoidKeyword)), "ModuleInit")
                        .AddModifiers(Token(InternalKeyword), Token(StaticKeyword))
                        .AddAttributeLists(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("ModuleInitializer")))))
                        .WithBody(Block(GenerateExportStatements(reduced)))))
            .NormalizeWhitespace();
    }

    private static IEnumerable<StatementSyntax> GenerateExportStatements(List<(string Type, string ImplType, string Model)> exports)
    {
        return exports.OrderBy(d => d.Type).ThenBy(d => d.ImplType).ThenBy(d => d.Model)
            .Select(d => ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(SimpleMemberAccessExpression,
                        GenericName(Identifier("DeviceFactory"), TypeArgumentList(SingletonSeparatedList(ParseTypeName(d.Type)))),
                        GenericName(Identifier("Register"), TypeArgumentList(SingletonSeparatedList(ParseTypeName(d.ImplType))))),
                    ArgumentList(SingletonSeparatedList(Argument(LiteralExpression(StringLiteralExpression, Literal(d.Model))))))));
    }
}