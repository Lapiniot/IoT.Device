using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IoT.Device.Generators;

internal class FilterExportAttributesSyntaxContextReceiver : ISyntaxContextReceiver
{
    private const string ExportAttributeName = "ExportAttribute";
    private const string AssemblyName = "IoT.Device";
    private const string DeviceNsName = "Device";
    private const string IoTNsName = "IoT";

    public List<(string Type, string ImplType, string Model)> Exports { get; } = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if(context.Node is not AttributeSyntax attribute)
        {
            return;
        }

        if(attribute is { Parent: AttributeListSyntax { Parent: CompilationUnitSyntax }, ArgumentList: { Arguments: { Count: > 0 } arguments } })
        {
            var type = context.SemanticModel.GetTypeInfo(attribute).Type as INamedTypeSymbol;

            if(type is
                {
                    IsGenericType: true,
                    IsUnboundGenericType: false,
                    BaseType:
                    {
                        IsGenericType: true,
                        Name: ExportAttributeName,
                        ContainingAssembly.Name: AssemblyName,
                        ContainingNamespace: { Name: DeviceNsName, ContainingNamespace: { Name: IoTNsName, ContainingNamespace.IsGlobalNamespace: true } },
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
        else if(attribute is { Parent: AttributeListSyntax { Parent: TypeDeclarationSyntax parent }, ArgumentList: { Arguments: { Count: > 0 } args } })
        {
            if(context.SemanticModel.GetTypeInfo(attribute).Type is INamedTypeSymbol
                {
                    IsGenericType: false,
                    BaseType:
                    {
                        IsGenericType: true,
                        Name: ExportAttributeName,
                        ContainingAssembly.Name: AssemblyName,
                        ContainingNamespace: { Name: DeviceNsName, ContainingNamespace: { Name: IoTNsName, ContainingNamespace.IsGlobalNamespace: true } },
                        TypeArguments: { Length: 1 } typeArguments
                    }
                } &&
                context.SemanticModel.GetDeclaredSymbol(parent) is ITypeSymbol implType &&
                context.SemanticModel.GetConstantValue(args[0].Expression) is { HasValue: true, Value: string { } value })
            {
                Exports.Add((typeArguments[0].ToDisplayString(), implType.ToDisplayString(), value));
            }
        }
    }
}