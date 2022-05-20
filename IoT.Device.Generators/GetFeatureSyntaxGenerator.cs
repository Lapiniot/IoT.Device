using IoT.Device.Generators.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class GetFeatureSyntaxGenerator
{
    public static SyntaxNode GenerateAugmentation(ITypeSymbol @class, INamedTypeSymbol[] attributes)
    {
        var allTypes = attributes.SelectMany(a => ExtractRelatedTypes(a.TypeArguments)).Distinct<ITypeSymbol>(SymbolEqualityComparer.Default);
        var resolved = NameHelper.ResolveTypeNames(allTypes, out var imports);

        var implementationTypes = attributes.Select(a => a.TypeArguments.Length > 1 ? a.TypeArguments[1] : a.TypeArguments[0]).
            Distinct<ITypeSymbol>(SymbolEqualityComparer.Default).
            Select(t => resolved[t]);

        var conditions = attributes.Select(a => a switch
            {
                { TypeArguments: { Length: 1 } args } => (Types: args[0].EnumerateRelatedFeatureTypes().Select(t => resolved[t]), ImplType: resolved[args[0]]),
                { TypeArguments: { Length: 2 } dargs } => (Types: dargs[0].EnumerateRelatedFeatureTypes().Select(t => resolved[t]), ImplType: resolved[dargs[1]]),
                _ => default
            });

        return NamespaceDeclaration(ParseName(@class.ContainingNamespace.ToDisplayString()))
            .AddUsings(imports.Select(i => UsingDirective(ParseName(i))).ToArray())
            .AddMembers(ClassDeclaration(@class.Name).AddModifiers(Token(PublicKeyword), Token(PartialKeyword))
                .AddMembers(implementationTypes.Select(a => CreateFeatureInstanceField(a)).ToArray())
                .AddMembers(MethodDeclaration(IdentifierName("T"), Identifier("GetFeature"))
                    .AddTypeParameterListParameters(TypeParameter(Identifier("T")))
                    .AddModifiers(Token(PublicKeyword), Token(OverrideKeyword))
                    .WithBody(Block(GenerateGetFeatureBody(conditions, invokeBaseImpl: @class.BaseType.HasOverrideForGetFeatureMethod())))))
            .NormalizeWhitespace();
    }

    private static IEnumerable<ITypeSymbol> ExtractRelatedTypes(IEnumerable<ITypeSymbol> typeArguments)
    {
        var order = 0;
        foreach (var argType in typeArguments)
        {
            if (order++ == 0)
            {
                foreach (var type in argType.EnumerateRelatedFeatureTypes())
                {
                    yield return type;
                }
            }
            else
            {
                yield return argType;
            }
        }
    }

    private static MemberDeclarationSyntax CreateFeatureInstanceField(string typeName) =>
        FieldDeclaration(VariableDeclaration(
                ParseTypeName(typeName),
                SingletonSeparatedList(VariableDeclarator(Identifier(GetFeatureFieldName(typeName))))))
            .AddModifiers(Token(PrivateKeyword));

    private static string GetFeatureFieldName(string typeName) =>
        $"{char.ToLowerInvariant(typeName[0])}{typeName.Substring(1).Replace(".", "")}Feature";

    private static IEnumerable<StatementSyntax> GenerateGetFeatureBody(IEnumerable<(IEnumerable<string> Types, string ImplType)> conditions, bool invokeBaseImpl)
    {
        yield return LocalDeclarationStatement(VariableDeclaration(ParseTypeName("Type"), SingletonSeparatedList(VariableDeclarator(Identifier("type"), null, EqualsValueClause(TypeOfExpression(ParseTypeName("T")))))));

        foreach (var (types, implType) in conditions)
        {
            yield return GenerateTypeTestConditionBlock(types, implType);
        }

        yield return ReturnStatement(invokeBaseImpl
            ? InvocationExpression(MemberAccessExpression(
                SimpleMemberAccessExpression,
                BaseExpression(),
                GenericName(Identifier("GetFeature"), TypeArgumentList(SingletonSeparatedList(ParseTypeName("T"))))))
            : LiteralExpression(NullLiteralExpression));
    }

    private static StatementSyntax GenerateTypeTestConditionBlock(IEnumerable<string> featureTypes, string implType) =>
        IfStatement(
            GenerateTypeTestCondition(featureTypes),
            Block(ReturnStatement(
                BinaryExpression(
                    AsExpression,
                    ParenthesizedExpression(AssignmentExpression(
                        CoalesceAssignmentExpression,
                        IdentifierName(GetFeatureFieldName(implType)),
                        ObjectCreationExpression(
                            ParseTypeName(implType),
                            ArgumentList(SingletonSeparatedList(Argument(ThisExpression()))), null))),
                    ParseTypeName("T")))));

    private static ExpressionSyntax GenerateTypeTestCondition(IEnumerable<string> types)
    {
        ExpressionSyntax? expression = null;
        foreach (var type in types)
        {
            expression = expression is null
                ? BinaryExpression(
                    EqualsExpression,
                    IdentifierName("type"),
                    TypeOfExpression(ParseTypeName(type)))
                : BinaryExpression(
                    LogicalOrExpression,
                    BinaryExpression(
                        EqualsExpression,
                        IdentifierName("type"),
                        TypeOfExpression(ParseTypeName(type))),
                    expression);
        }

        return expression ?? LiteralExpression(FalseLiteralExpression);
    }
}