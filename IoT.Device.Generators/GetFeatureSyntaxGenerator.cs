using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace IoT.Device.Generators;

internal record struct ConditionData(string ImplType, string FieldName, IReadOnlyCollection<string> FeatureTypes);

internal static class GetFeatureSyntaxGenerator
{
    public static string GenerateAugmentation(string className, string namespaceName, bool invokeBaseImpl,
        IReadOnlyDictionary<string, string> fields, IReadOnlyCollection<ConditionData> conditions) =>
        NamespaceDeclaration(ParseName(namespaceName))
            .AddMembers(ClassDeclaration(className).AddModifiers(Token(PublicKeyword), Token(PartialKeyword))
                .AddMembers(fields.Select(pair => CreateFeatureInstanceField(pair.Value, pair.Key)).ToArray())
                .AddMembers(MethodDeclaration(IdentifierName("T"), Identifier("GetFeature"))
                    .AddTypeParameterListParameters(TypeParameter(Identifier("T")))
                    .AddModifiers(Token(PublicKeyword), Token(OverrideKeyword))
                    .WithBody(Block(GenerateGetFeatureBody(conditions, invokeBaseImpl)))))
            .NormalizeWhitespace().ToFullString();

    private static MemberDeclarationSyntax CreateFeatureInstanceField(string fieldName, string typeName) =>
        FieldDeclaration(VariableDeclaration(
                ParseTypeName(typeName),
                SingletonSeparatedList(VariableDeclarator(Identifier(fieldName)))))
            .AddModifiers(Token(PrivateKeyword));

    private static IEnumerable<StatementSyntax> GenerateGetFeatureBody(IEnumerable<ConditionData> conditions, bool invokeBaseImpl)
    {
        yield return LocalDeclarationStatement(VariableDeclaration(ParseTypeName("Type"),
            SingletonSeparatedList(VariableDeclarator(Identifier("type"), null, EqualsValueClause(TypeOfExpression(ParseTypeName("T")))))));

        foreach (var (implType, fieldName, featureTypes) in conditions)
        {
            yield return GenerateTypeTestConditionBlock(featureTypes, implType, fieldName);
        }

        yield return ReturnStatement(invokeBaseImpl
            ? InvocationExpression(MemberAccessExpression(
                SimpleMemberAccessExpression,
                BaseExpression(),
                GenericName(Identifier("GetFeature"), TypeArgumentList(SingletonSeparatedList(ParseTypeName("T"))))))
            : LiteralExpression(NullLiteralExpression));
    }

    private static StatementSyntax GenerateTypeTestConditionBlock(IEnumerable<string> featureTypes, string implType, string fieldName) =>
        IfStatement(
            GenerateTypeTestCondition(featureTypes),
            Block(ReturnStatement(
                BinaryExpression(
                    AsExpression,
                    ParenthesizedExpression(AssignmentExpression(
                        CoalesceAssignmentExpression,
                        IdentifierName(fieldName),
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