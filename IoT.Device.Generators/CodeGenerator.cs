using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class CodeGenerator
{
    public static SyntaxNode GenerateLibInitClass(string namespaceName, string className = "Library",
        string initMethodName = "Init", BlockSyntax? initMethodBody = null)
    {
        return NamespaceDeclaration(ParseName(namespaceName))
            .AddUsings(UsingDirective(ParseName("System.Runtime.CompilerServices")))
            .AddUsings(UsingDirective(ParseName("IoT.Device")))
            .AddMembers(ClassDeclaration(className)
                .AddModifiers(Token(PublicKeyword), Token(StaticKeyword))
                .AddMembers(
                    MethodDeclaration(PredefinedType(Token(VoidKeyword)), initMethodName)
                        .AddModifiers(Token(PublicKeyword), Token(StaticKeyword))
                        .WithBody(Block(ParseStatement(string.Empty))),
                    MethodDeclaration(PredefinedType(Token(VoidKeyword)), "ModuleInit")
                        .AddModifiers(Token(InternalKeyword), Token(StaticKeyword))
                        .AddAttributeLists(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("ModuleInitializer")))))
                        .WithBody(initMethodBody ?? Block(ParseStatement(string.Empty)))))
            .NormalizeWhitespace();
    }

    public static BlockSyntax GenerateExportStatements(List<(string Type, string ImplType, string Model)> exports)
    {
        return Block(exports.OrderBy(d => d.Type).ThenBy(d => d.ImplType).ThenBy(d => d.Model)
            .Select(d => ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(SimpleMemberAccessExpression,
                        GenericName(Identifier("DeviceFactory"), TypeArgumentList(SingletonSeparatedList(ParseTypeName(d.Type)))),
                        GenericName(Identifier("Register"), TypeArgumentList(SingletonSeparatedList(ParseTypeName(d.ImplType))))),
                    ArgumentList(SingletonSeparatedList(Argument(LiteralExpression(StringLiteralExpression, Literal(d.Model)))))))));
    }
}