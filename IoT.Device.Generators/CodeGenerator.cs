using Microsoft.CodeAnalysis;
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
}