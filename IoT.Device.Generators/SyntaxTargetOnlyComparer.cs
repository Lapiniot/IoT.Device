using Microsoft.CodeAnalysis;

namespace IoT.Device.Generators;

public class SyntaxTargetOnlyComparer<T> : IEqualityComparer<(T? Syntax, Compilation)> where T : SyntaxNode
{
    public bool Equals((T? Syntax, Compilation) x, (T? Syntax, Compilation) y) =>
        EqualityComparer<T?>.Default.Equals(x.Syntax, y.Syntax);

    public int GetHashCode((T? Syntax, Compilation) obj) =>
        EqualityComparer<T?>.Default.GetHashCode(obj.Syntax);
}