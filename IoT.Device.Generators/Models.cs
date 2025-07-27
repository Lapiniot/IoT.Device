using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace IoT.Device.Generators;

internal readonly record struct ExportContext(string TargetType, string ImplType, string ModelName);

internal readonly record struct DiagnosticContext(DiagnosticDescriptor Descriptor, LocationContext Location);

internal readonly record struct LocationContext(string FilePath, TextSpan SourceSpan, LinePositionSpan LineSpan)
{
    public static LocationContext Create(Location location) =>
        new(location.SourceTree?.FilePath ?? "", location.SourceSpan, location.GetLineSpan().Span);
}