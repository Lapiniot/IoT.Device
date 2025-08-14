using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace IoT.Device.Generators;

internal readonly record struct ExportContext(string TargetType, string ImplType, string ModelName);

internal readonly record struct FeatureSourceContext(string TypeName,
    string NamespaceName, bool ShoulCallBaseImpl, ImmutableArray<FeatureContext> Features) :
    IEquatable<FeatureSourceContext>
{
    public bool Equals(FeatureSourceContext other) =>
        EqualityComparer<string>.Default.Equals(TypeName, other.TypeName) &&
        EqualityComparer<string>.Default.Equals(NamespaceName, other.NamespaceName) &&
        EqualityComparer<bool>.Default.Equals(ShoulCallBaseImpl, other.ShoulCallBaseImpl) &&
        Features.SequenceEqual(other.Features, EqualityComparer<FeatureContext>.Default);

    public override int GetHashCode()
    {
        HashCode hashCode = default;
        hashCode.Add(TypeName);
        hashCode.Add(NamespaceName);
        hashCode.Add(ShoulCallBaseImpl);
        foreach (var item in Features)
        {
            hashCode.Add(item);
        }

        return hashCode.ToHashCode();
    }
}

internal readonly record struct FeatureContext(string TypeName, string? ImplTypeName);

internal readonly record struct DiagnosticContext(DiagnosticDescriptor Descriptor, LocationContext Location)
{
    public Diagnostic ToDiagnostic() => Diagnostic.Create(Descriptor,
        Microsoft.CodeAnalysis.Location.Create(Location.FilePath,
            Location.SourceSpan, Location.LineSpan));
}

internal readonly record struct LocationContext(string FilePath, TextSpan SourceSpan, LinePositionSpan LineSpan)
{
    public static LocationContext Create(Location location) =>
        new(location.SourceTree?.FilePath ?? "", location.SourceSpan, location.GetLineSpan().Span);
}