using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IoT.Device.Generators;

internal class FilterExportAttributesSyntaxContextReceiver : ISyntaxContextReceiver
{
    public List<(string Type, string ImplType, string Model)> Exports { get; } = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if(context.Node is AttributeSyntax
            {
                Parent: AttributeListSyntax { Parent: CompilationUnitSyntax },
                ArgumentList: { Arguments: { Count: > 0 } arguments }
            } attribute)
        {
            var type = context.SemanticModel.GetTypeInfo(attribute).Type as INamedTypeSymbol;

            if(type is
                {
                    IsGenericType: true,
                    IsUnboundGenericType: false,
                    BaseType:
                    {
                        IsGenericType: true,
                        Name: "ExportAttribute",
                        ContainingAssembly.Name: "IoT.Device",
                        ContainingNamespace: { Name: "Device", ContainingNamespace: { Name: "IoT" } },
                        TypeArguments: { Length: 1 } typeArguments
                    },
                    TypeParameters: { Length: 1 } typeParameters
                } &&
                typeArguments[0] is INamedTypeSymbol argument &&
                typeParameters[0] is { ConstraintTypes: { Length: > 0 } cts } &&
                cts[0] is INamedTypeSymbol constraint &&
                context.SemanticModel.GetConstantValue(arguments[0].Expression) is { HasValue: true, Value: string { } value })
            {
                Exports.Add((constraint.ToDisplayString(), argument.ToDisplayString(), value));
            }
        }
    }
}