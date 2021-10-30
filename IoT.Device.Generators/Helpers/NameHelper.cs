using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using TreeNode = IoT.Device.Generators.Helpers.HashTreeNode<string, (string Ns, Microsoft.CodeAnalysis.ITypeSymbol? Symbol)>;

namespace IoT.Device.Generators.Helpers;

internal static class NameHelper
{
    [SuppressMessage("Roslyn", "RS1024: Compare symbols correctly", Justification = "False positive due to the buggy analyzer")]
    public static IDictionary<ISymbol, string> ResolveTypeNames(IEnumerable<ITypeSymbol> typeSymbols, out IEnumerable<string> namespaces)
    {
        var ns = new HashSet<string>();
        var map = new Dictionary<ISymbol, string>(SymbolEqualityComparer.Default);
        var lookup = typeSymbols.ToLookup(s => s.Name);

        foreach(var group in lookup)
        {
            var symbols = group.ToList();

            if(symbols.Count == 1)
            {
                var symbol = symbols[0];
                _ = ns.Add(symbol.ContainingNamespace.ToDisplayString());
                map[symbol] = symbol.Name;
            }
            else
            {
                var reducedTypeNames = ResolveAmbiguousNames(symbols, out var resolvedNamespaces);

                foreach(var (symbol, shortName) in reducedTypeNames)
                {
                    map[symbol] = shortName;
                }

                foreach(var @namespace in resolvedNamespaces)
                {
                    _ = ns.Add(@namespace);
                }
            }
        }

        namespaces = ns;
        return map;
    }

    public static IEnumerable<(ITypeSymbol Symbol, string ShortName)> ResolveAmbiguousNames(
        IEnumerable<ITypeSymbol> symbols, out IEnumerable<string> resolvedNamespaces)
    {
        var list = new List<(ITypeSymbol, string)>();
        var ns = new HashSet<string>();

        var root = BuildNamespacesHierarchy(symbols);

        foreach(var current in root.TraverseTree())
        {
            if(current.Value.Symbol is { Name: { } name } symbol)
            {
                // node is terminal, emit current type name + containing namespace
                var prefix = string.Join(".", current.Path.TakeWhile(p => p != root && p.Count <= 1).Reverse().Select(p => p.Value.Ns));
                var nspace = string.Join(".", current.Path.SkipWhile(p => p.Count <= 1).Reverse().Skip(1).Select(p => p.Value.Ns));
                if(!string.IsNullOrEmpty(nspace)) _ = ns.Add(nspace);
                list.Add((symbol, !string.IsNullOrEmpty(prefix) ? $"{prefix}.{symbol.Name}" : symbol.Name));
            }
        }

        resolvedNamespaces = ns;
        return list;
    }

    public static TreeNode BuildNamespacesHierarchy(IEnumerable<ITypeSymbol> symbols)
    {
        var root = new TreeNode();

        foreach(var symbol in symbols)
        {
            var node = root;
            foreach(var token in symbol.ContainingNamespace.ToDisplayParts())
            {
                if(token is { Kind: SymbolDisplayPartKind.NamespaceName, Symbol: { Name: { } name } })
                {
                    node = node.GetOrAdd(name, _ => new TreeNode((name, null)));
                }
            }
            node.Value = (symbol.ContainingNamespace.Name, symbol);
        }

        return root;
    }
}