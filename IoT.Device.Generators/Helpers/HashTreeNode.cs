using System.Collections;

namespace IoT.Device.Generators.Helpers;

internal class HashTreeNode<TKey, TValue> : IEnumerable<HashTreeNode<TKey, TValue>>
{
    private readonly Dictionary<TKey, HashTreeNode<TKey, TValue>> store = new();

    public HashTreeNode() { }

    public HashTreeNode(TValue value)
    {
        Value = value;
    }

    public TValue? Value { get; set; }

    public HashTreeNode<TKey, TValue>? Parent { get; private set; }

    public HashTreeNode<TKey, TValue>? this[TKey key]
    {
        get => store.TryGetValue(key, out var value) ? value : null;
        set
        {
            if(value is not null)
            {
                value.Parent = this;
                store[key] = value;
            }
            else
            {
                if(store.Remove(key, out var node))
                {
                    node.Parent = null;
                }
            }
        }
    }

    public HashTreeNode<TKey, TValue> GetOrAdd(TKey key, Func<TKey, HashTreeNode<TKey, TValue>> factory)
    {
        if(factory is null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        return this[key] ?? (this[key] = factory(key));
    }

    public int Count => store.Count;

    public void Clear()
    {
        foreach(var node in store.Values)
        {
            node.Parent = null;
        }
        store.Clear();
    }

    public IEnumerator<HashTreeNode<TKey, TValue>> GetEnumerator()
    {
        return store.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<HashTreeNode<TKey, TValue>> Path
    {
        get
        {
            yield return this;
            var parent = Parent;
            while(parent is not null)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }
    }

    public IEnumerable<HashTreeNode<TKey, TValue>> TraverseTree()
    {
        var stack = new Stack<HashTreeNode<TKey, TValue>>();
        stack.Push(this);
        while(stack.TryPop(out var current))
        {
            yield return current;
            foreach(var child in current)
            {
                stack.Push(child);
            }
        }
    }
}