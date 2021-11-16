using Microsoft.CodeAnalysis;

namespace IoT.Device.Generators;

public record ExportDescriptor(ITypeSymbol Type, ITypeSymbol ImplType, string Model);