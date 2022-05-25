using IoT.Device.Generators.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal static class GetFeatureSyntaxGenerator
{
    public static SyntaxNode GenerateAugmentation(string className, string namespaceName, IReadOnlyCollection<INamedTypeSymbol> attributes, bool invokeBaseImpl)
    {
        var implementationTypes = attributes.Select(a => a.TypeArguments.Length > 1 ? a.TypeArguments[1] : a.TypeArguments[0]).Distinct<ITypeSymbol>(SymbolEqualityComparer.Default);

        var conditions = attributes.Select(a => a switch
        {
            { TypeArguments: { Length: 1 } args } => (Types: args[0].EnumerateRelatedFeatureTypes(), ImplType: args[0]),
            { TypeArguments: { Length: 2 } dargs } => (Types: dargs[0].EnumerateRelatedFeatureTypes(), ImplType: dargs[1]),
            _ => default
        });

        return NamespaceDeclaration(ParseName(namespaceName))
            .AddMembers(ClassDeclaration(className).AddModifiers(Token(PublicKeyword), Token(PartialKeyword))
                .AddMembers(implementationTypes.Select(a => CreateFeatureInstanceField(
                    GetFeatureFieldName(a.Name), a.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))).ToArray())
                .AddMembers(MethodDeclaration(IdentifierName("T"), Identifier("GetFeature"))
                    .AddTypeParameterListParameters(TypeParameter(Identifier("T")))
                    .AddModifiers(Token(PublicKeyword), Token(OverrideKeyword))
                    .WithBody(Block(GenerateGetFeatureBody(conditions, invokeBaseImpl)))))
            .NormalizeWhitespace();
    }

    private static MemberDeclarationSyntax CreateFeatureInstanceField(string fieldName, string typeName) =>
        FieldDeclaration(VariableDeclaration(
                ParseTypeName(typeName),
                SingletonSeparatedList(VariableDeclarator(Identifier(fieldName)))))
            .AddModifiers(Token(PrivateKeyword));

    private static string GetFeatureFieldName(string typeName) =>
        $"{char.ToLowerInvariant(typeName[0])}{typeName.Substring(1).Replace(".", "")}Feature";

    private static IEnumerable<StatementSyntax> GenerateGetFeatureBody(IEnumerable<(IEnumerable<ITypeSymbol> Types, ITypeSymbol ImplType)> conditions, bool invokeBaseImpl)
    {
        yield return LocalDeclarationStatement(VariableDeclaration(ParseTypeName("Type"),
            SingletonSeparatedList(VariableDeclarator(Identifier("type"), null, EqualsValueClause(TypeOfExpression(ParseTypeName("T")))))));

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

    private static StatementSyntax GenerateTypeTestConditionBlock(IEnumerable<ITypeSymbol> featureTypes, ITypeSymbol implType) =>
        IfStatement(
            GenerateTypeTestCondition(featureTypes),
            Block(ReturnStatement(
                BinaryExpression(
                    AsExpression,
                    ParenthesizedExpression(AssignmentExpression(
                        CoalesceAssignmentExpression,
                        IdentifierName(GetFeatureFieldName(implType.Name)),
                        ObjectCreationExpression(
                            ParseTypeName(implType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)),
                            ArgumentList(SingletonSeparatedList(Argument(ThisExpression()))), null))),
                    ParseTypeName("T")))));

    private static ExpressionSyntax GenerateTypeTestCondition(IEnumerable<ITypeSymbol> types)
    {
        ExpressionSyntax? expression = null;
        foreach (var type in types)
        {
            expression = expression is null
                ? BinaryExpression(
                    EqualsExpression,
                    IdentifierName("type"),
                    TypeOfExpression(ParseTypeName(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))))
                : BinaryExpression(
                    LogicalOrExpression,
                    BinaryExpression(
                        EqualsExpression,
                        IdentifierName("type"),
                        TypeOfExpression(ParseTypeName(type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))),
                    expression);
        }

        return expression ?? LiteralExpression(FalseLiteralExpression);
    }
}