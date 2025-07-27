using Microsoft.CodeAnalysis;

namespace IoT.Device.Generators;

public static class ExportAttributeSyntaxParser
{
    private const string ExportAttributeName = "ExportAttribute";
    private const string AssemblyName = "IoT.Device";
    private const string DeviceNsName = "Device";
    private const string IoTNsName = "IoT";

    public static bool TryGetExportAttribute(INamedTypeSymbol symbol,
        out ITypeSymbol? targetType, out string? modelName,
        CancellationToken cancellationToken)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (attribute is
                {
                    AttributeClass:
                    {
                        IsGenericType: false,
                        BaseType:
                        {
                            IsGenericType: true,
                            Name: ExportAttributeName,
                            ContainingAssembly.Name: AssemblyName,
                            ContainingNamespace:
                            {
                                Name: DeviceNsName,
                                ContainingNamespace:
                                {
                                    Name: IoTNsName,
                                    ContainingNamespace.IsGlobalNamespace: true
                                }
                            },
                            TypeArguments: [var typeArg, ..]
                        }
                    },
                    ConstructorArguments: [{ Type.SpecialType: SpecialType.System_String, Value: string model }, ..]
                })
            {
                targetType = typeArg;
                modelName = model;
                return true;
            }
        }

        targetType = null;
        modelName = null;
        return false;
    }
}