using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IoT.Device.Generators;

public static class ExportAttributeSyntaxParser
{
    private const string ExportAttributeName = "ExportAttribute";
    private const string AssemblyName = "IoT.Device";
    private const string DeviceNsName = "Device";
    private const string IoTNsName = "IoT";

    public static bool IsSuitableCandidate(SyntaxNode node)
    {
        return node is AttributeSyntax
        {
            Parent: AttributeListSyntax { Parent: ClassDeclarationSyntax or CompilationUnitSyntax },
            ArgumentList: { Arguments.Count: > 0 }
        };
    }

    public static ExportDescriptor? Parse(AttributeSyntax attribute, SemanticModel model, CancellationToken cancellationToken)
    {
        if(model is null) throw new ArgumentNullException(nameof(model));

        return attribute switch
        {
            { Parent: AttributeListSyntax { Parent: CompilationUnitSyntax }, ArgumentList: { Arguments: { Count: > 0 } } }
                => ParseAsAssemblyAttribute(attribute, model, cancellationToken),
            { Parent: AttributeListSyntax { Parent: ClassDeclarationSyntax }, ArgumentList: { Arguments: { Count: > 0 } } }
                => ParseAsClassAttribute(attribute, model, cancellationToken),
            _ => null
        };
    }

    private static ExportDescriptor? ParseAsClassAttribute(AttributeSyntax attribute, SemanticModel model, CancellationToken cancellationToken)
    {
        return model.GetTypeInfo(attribute, cancellationToken).Type is INamedTypeSymbol
        {
            IsGenericType: false,
            BaseType:
            {
                IsGenericType: true,
                Name: ExportAttributeName,
                ContainingAssembly.Name: AssemblyName,
                ContainingNamespace: { Name: DeviceNsName, ContainingNamespace: { Name: IoTNsName, ContainingNamespace.IsGlobalNamespace: true } },
                TypeArguments: { Length: 2 } typeArgs
            }
        } &&
        model.GetDeclaredSymbol(attribute.Parent!.Parent!, cancellationToken) is ITypeSymbol implType &&
        model.GetConstantValue(attribute.ArgumentList!.Arguments[0].Expression, cancellationToken) is { HasValue: true, Value: string { } value }
            ? new(typeArgs[0], implType, value)
            : null;
    }

    private static ExportDescriptor? ParseAsAssemblyAttribute(AttributeSyntax attribute, SemanticModel model, CancellationToken cancellationToken)
    {
        return model.GetTypeInfo(attribute, cancellationToken).Type is INamedTypeSymbol
        {
            IsGenericType: true,
            IsUnboundGenericType: false,
            BaseType:
            {
                IsGenericType: true,
                Name: ExportAttributeName,
                ContainingAssembly.Name: AssemblyName,
                ContainingNamespace: { Name: DeviceNsName, ContainingNamespace: { Name: IoTNsName, ContainingNamespace.IsGlobalNamespace: true } },
                TypeArguments: { Length: 2 } typeArgs
            }
        } &&
        model.GetConstantValue(attribute.ArgumentList!.Arguments[0].Expression, cancellationToken) is { HasValue: true, Value: string { } value }
            ? new(typeArgs[0], typeArgs[1], value)
            : null;
    }
}