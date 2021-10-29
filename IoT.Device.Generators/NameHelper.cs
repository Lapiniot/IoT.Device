using Microsoft.CodeAnalysis;
using TreeNode = IoT.Device.Generators.HashTreeNode<string, (string Ns, Microsoft.CodeAnalysis.ITypeSymbol? Symbol)>;

namespace IoT.Device.Generators;

internal static class NameHelper
{
    public static List<(string Type, string ImplType, string Model)> ExtractNames(
        List<(ITypeSymbol Type, ITypeSymbol ImplType, string Model)> exports,
        out List<string> namespaces)
    {
        var ns = new HashSet<string>();
        var list = new List<(string Type, string ImplType, string Model)>();

#pragma warning disable RS1024
        var map = new Dictionary<ISymbol, string>(SymbolEqualityComparer.Default);

        var types = exports.Select(e => e.Type)
            .Concat(exports.Select(e => e.ImplType))
            .Distinct<ITypeSymbol>(SymbolEqualityComparer.Default);

        var lookup = types.ToLookup(s => s.Name);
#pragma warning restore

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
                var reducedTypeNames = ReduceNames(symbols, out var resolvedNamespaces);

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

        foreach(var (type, implType, model) in exports)
        {
            if(map.TryGetValue(type, out var typeName) && map.TryGetValue(implType, out var implTypeName))
            {
                list.Add((typeName, implTypeName, model));
            }
        }

        namespaces = ns.ToList();
        return list;
    }

    internal static IEnumerable<(ITypeSymbol Symbol, string ShortName)> ReduceNames(IEnumerable<ITypeSymbol> symbols, out IEnumerable<string> resolvedNamespaces)
    {
        var list = new List<(ITypeSymbol, string)>();
        var ns = new List<string>();

        var tree = BuildNamespacesHierarchy(symbols);

        foreach(var current in tree.TraverseTree())
        {
            if(current.Value.Symbol is { Name: { } name } symbol)
            {
                // node is terminal, emit current type name + containing namespace
                ns.Add(string.Join(".", current.Path.Reverse().Skip(1).Select(n => n.Value.Ns)));
                list.Add((symbol, name));

                // also detach children and emit them as potential types in contained namespaces
                var detached = current.ToList();
                current.Clear();
                foreach(var node in detached)
                {
                    foreach(var child in node.TraverseTree())
                    {
                        if(child is { Value: { Symbol: { Name: { } n } s } })
                        {
                            list.Add((s, $"{string.Join(".", child.Path.Reverse().Select(n => n.Value.Ns))}.{n}"));
                        }
                    }
                }
                continue;
            }
        }

        resolvedNamespaces = ns;
        return list;
    }

    private static TreeNode BuildNamespacesHierarchy(IEnumerable<ITypeSymbol> symbols)
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