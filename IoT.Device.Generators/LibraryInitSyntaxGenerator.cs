using IoT.Device.Generators.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class LibraryInitSyntaxGenerator
{
    public static SyntaxNode GenerateLibInitClass(string namespaceName, string className, string initMethodName, IEnumerable<ExportDescriptor> exports)
    {
        var reduced = ReduceTypeNames(exports, out var namespaces);

        return NamespaceDeclaration(ParseName(namespaceName))
            .AddUsings(UsingDirective(ParseName("IoT.Device")))
            .AddUsings(namespaces.OrderBy(n => n).Select(ns => UsingDirective(ParseName(ns))).ToArray())
            .AddMembers(ClassDeclaration(className)
                .AddModifiers(Token(PublicKeyword), Token(StaticKeyword), Token(PartialKeyword))
                .AddMembers(
                    MethodDeclaration(PredefinedType(Token(VoidKeyword)), initMethodName)
                        .AddModifiers(Token(PublicKeyword), Token(StaticKeyword))
                        .WithBody(Block(GenerateExportStatements(reduced).Append(
                            ExpressionStatement(InvocationExpression(IdentifierName($"{initMethodName}Extra")))))),
                    MethodDeclaration(PredefinedType(Token(VoidKeyword)), $"{initMethodName}Extra")
                        .AddModifiers(Token(StaticKeyword), Token(PartialKeyword))
                        .WithSemicolonToken(Token(SemicolonToken))))
            .NormalizeWhitespace();
    }

    private static IEnumerable<StatementSyntax> GenerateExportStatements(IEnumerable<(string Type, string ImplType, string Model)> exports)
    {
        return exports.OrderBy(d => d.Type).ThenBy(d => d.ImplType).ThenBy(d => d.Model)
            .Select(d => ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(SimpleMemberAccessExpression,
                        GenericName(Identifier("DeviceFactory"), TypeArgumentList(SingletonSeparatedList(ParseTypeName(d.Type)))),
                        GenericName(Identifier("Register"), TypeArgumentList(SingletonSeparatedList(ParseTypeName(d.ImplType))))),
                    ArgumentList(SingletonSeparatedList(Argument(LiteralExpression(StringLiteralExpression, Literal(d.Model))))))));
    }

    private static IEnumerable<(string Type, string ImplType, string Model)> ReduceTypeNames(
        IEnumerable<ExportDescriptor> exports,
        out IEnumerable<string> namespaces)
    {
        var list = new List<(string Type, string ImplType, string Model)>();

        exports = exports.ToList();
        var types = exports.Select(e => e.Type)
            .Concat(exports.Select(e => e.ImplType))
            .Distinct<ITypeSymbol>(SymbolEqualityComparer.Default);

        var map = NameHelper.ResolveTypeNames(types, out var ns);

        foreach(var (type, implType, model) in exports)
        {
            if(map.TryGetValue(type, out var typeName) && map.TryGetValue(implType, out var implTypeName))
            {
                list.Add((typeName, implTypeName, model));
            }
        }

        namespaces = ns.ToList();
        return list;
    }
}