using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class GetFeatureSyntaxGenerator
{
    public static SyntaxNode GenerateAugmentation(ITypeSymbol @class, INamedTypeSymbol[] attributes)
    {
        var imports = attributes.SelectMany(a => ExtractTypeArgsNamespaces(a.TypeArguments)).Distinct().OrderBy(ns => ns);
        return NamespaceDeclaration(ParseName(@class.ContainingNamespace.ToDisplayString()))
            .AddUsings(imports.Select(i => UsingDirective(ParseName(i))).ToArray())
            .AddMembers(ClassDeclaration(@class.Name).AddModifiers(Token(PublicKeyword), Token(PartialKeyword))
                .AddMembers(attributes.Select(a => CreateFeatureInstanceField(a)).ToArray())
                .AddMembers(MethodDeclaration(IdentifierName("T"), Identifier("GetFeature"))
                    .AddTypeParameterListParameters(TypeParameter(Identifier("T")))
                    .AddModifiers(Token(PublicKeyword), Token(OverrideKeyword))
                    .WithBody(Block(GenerateGetFeatureBody(attributes, invokeBaseImpl: @class.BaseType.HasOverrideForGetFeatureMethod())))))
            .NormalizeWhitespace();
    }

    private static IEnumerable<string> ExtractTypeArgsNamespaces(IEnumerable<ITypeSymbol> typeArguments)
    {
        int order = 0;
        foreach(var argType in typeArguments)
        {
            if(order++ == 0)
            {
                foreach(var type in argType.EnumerateRelatedFeatureTypes())
                {
                    yield return type.ContainingNamespace.ToDisplayString();
                }
            }
            else
            {
                yield return argType.ContainingNamespace.ToDisplayString();
            }
        }
    }

    private static FieldDeclarationSyntax CreateFeatureInstanceField(INamedTypeSymbol attributeType, bool fullType = false)
    {
        var args = attributeType.TypeArguments;
        var type = args.Length > 1 ? args[1] : args[0];
        return FieldDeclaration(
            VariableDeclaration(
                ParseTypeName(fullType ? type.ToDisplayString() : type.Name),
                SingletonSeparatedList(VariableDeclarator(Identifier(GetFeatureFieldName(type.Name))))))
            .AddModifiers(Token(PrivateKeyword));
    }

    private static string GetFeatureFieldName(string typeName)
    {
        return $"{char.ToLowerInvariant(typeName[0])}{typeName[1..]}Feature";
    }

    private static IEnumerable<StatementSyntax> GenerateGetFeatureBody(IEnumerable<INamedTypeSymbol> attributes,
        bool fullTypeNames = false, bool invokeBaseImpl = false)
    {
        yield return LocalDeclarationStatement(VariableDeclaration(ParseTypeName("Type"), SingletonSeparatedList(VariableDeclarator(Identifier("type"), null, EqualsValueClause(TypeOfExpression(ParseTypeName("T")))))));

        foreach(var attr in attributes)
        {
            if(attr is { TypeArguments: { Length: 1 } args } &&
                args[0] is { Name: var shortName } type)
            {
                yield return IfStatement(
                    GenerateTypeTestCondition(type.EnumerateRelatedFeatureTypes(), fullTypeNames),
                    Block(ReturnStatement(BinaryExpression(
                        AsExpression,
                        ParenthesizedExpression(AssignmentExpression(
                            CoalesceAssignmentExpression,
                            IdentifierName(GetFeatureFieldName(shortName)),
                            ObjectCreationExpression(
                                ParseTypeName(fullTypeNames ? type.ToDisplayString() : shortName),
                                ArgumentList(SingletonSeparatedList(Argument(ThisExpression()))), null))),
                        ParseTypeName("T")))));
            }
        }

        yield return ReturnStatement(invokeBaseImpl
            ? InvocationExpression(MemberAccessExpression(
                SimpleMemberAccessExpression,
                BaseExpression(),
                GenericName(Identifier("GetFeature"), TypeArgumentList(SingletonSeparatedList(ParseTypeName("T"))))))
            : LiteralExpression(NullLiteralExpression));
    }

    private static ExpressionSyntax GenerateTypeTestCondition(IEnumerable<ITypeSymbol> types, bool fullTypes)
    {
        ExpressionSyntax? expression = null;
        foreach(var item in types)
        {
            expression = expression is null
                ? BinaryExpression(
                    EqualsExpression,
                    IdentifierName("type"),
                    TypeOfExpression(ParseTypeName(fullTypes ? item.ToDisplayString() : item.Name)))
                : BinaryExpression(
                    LogicalOrExpression,
                    BinaryExpression(
                        EqualsExpression,
                        IdentifierName("type"),
                        TypeOfExpression(ParseTypeName(fullTypes ? item.ToDisplayString() : item.Name))),
                    expression);
        }
        return expression ?? LiteralExpression(FalseLiteralExpression);
    }
}