using Microsoft.CodeAnalysis;

namespace IoT.Device.Generators;

internal record ExportDescriptor(ITypeSymbol Type, ITypeSymbol ImplType, string Model);