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
        var implementationTypes = attributes.Select(a => a.TypeArguments.Length > 1 ? a.TypeArguments[1] : a.TypeArguments[0]).
            Distinct<ITypeSymbol>(SymbolEqualityComparer.Default);

        return NamespaceDeclaration(ParseName(@class.ContainingNamespace.ToDisplayString()))
            .AddUsings(imports.Select(i => UsingDirective(ParseName(i))).ToArray())
            .AddMembers(ClassDeclaration(@class.Name).AddModifiers(Token(PublicKeyword), Token(PartialKeyword))
                .AddMembers(implementationTypes.Select(a => CreateFeatureInstanceField(a)).ToArray())
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

    private static FieldDeclarationSyntax CreateFeatureInstanceField(ITypeSymbol type, bool fullType = false)
    {
        return FieldDeclaration(VariableDeclaration(
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
            if(attr is { TypeArguments: { Length: 1 } args })
            {
                yield return GenerateTypeTestConditionBlock(args[0].EnumerateRelatedFeatureTypes(), args[0], fullTypeNames);
            }
            else if(attr is { TypeArguments: { Length: 2 } dargs })
            {
                yield return GenerateTypeTestConditionBlock(dargs[0].EnumerateRelatedFeatureTypes(), dargs[1], fullTypeNames);
            }
        }

        yield return ReturnStatement(invokeBaseImpl
            ? InvocationExpression(MemberAccessExpression(
                SimpleMemberAccessExpression,
                BaseExpression(),
                GenericName(Identifier("GetFeature"), TypeArgumentList(SingletonSeparatedList(ParseTypeName("T"))))))
            : LiteralExpression(NullLiteralExpression));
    }

    private static StatementSyntax GenerateTypeTestConditionBlock(IEnumerable<ITypeSymbol> featureTypes, ITypeSymbol implType, bool fullTypeNames)
    {
        return IfStatement(
            GenerateTypeTestCondition(featureTypes, fullTypeNames),
            Block(ReturnStatement(
                BinaryExpression(
                    AsExpression,
                    ParenthesizedExpression(AssignmentExpression(
                        CoalesceAssignmentExpression,
                        IdentifierName(GetFeatureFieldName(implType.Name)),
                        ObjectCreationExpression(
                            ParseTypeName(fullTypeNames ? implType.ToDisplayString() : implType.Name),
                            ArgumentList(SingletonSeparatedList(Argument(ThisExpression()))), null))),
                    ParseTypeName("T")))));
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