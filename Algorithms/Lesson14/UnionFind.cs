//测试通过

namespace Algorithms.Lesson14;

public class Node<T>
{
    private readonly T _value;

    public Node(T v)
    {
        _value = v;
    }

    public T GetValue()
    {
        return _value;
    }
}

public class UnionFind<T> where T : notnull
{
    //每个元素与包装对象的对应关系
    private readonly Dictionary<T, Node<T>> _nodes;

    //包装对象与集合代表对象的对应关系
    private readonly Dictionary<Node<T>, Node<T>> _representatives;

    //只含有每个集合的代表元素与所在集合的大小的对应关系
    private readonly Dictionary<Node<T>, int> _sizeMap;

    public UnionFind(List<T> values)
    {
        _nodes = new Dictionary<T, Node<T>>();
        _representatives = new Dictionary<Node<T>, Node<T>>();
        _sizeMap = new Dictionary<Node<T>, int>();
        foreach (var cur in values)
        {
            Node<T> node = new(cur);
            _nodes.Add(cur, node);
            _representatives.Add(node, node);
            _sizeMap.Add(node, 1);
        }
    }

    // 给你一个节点，请你往上到不能再往上，把代表返回
    private Node<T> FindRepresentative(Node<T> cur)
    {
        Stack<Node<T>> path = new();
        while (cur != _representatives[cur])
        {
            path.Push(cur);
            cur = _representatives[cur];
        }

        while (path.Count != 0) _representatives[path.Pop()] = cur;

        return cur;
    }

    public bool IsSameSet(T a, T b)
    {
        return FindRepresentative(_nodes[a]) == FindRepresentative(_nodes[b]);
    }

    public void Union(T a, T b)
    {
        var aHead = FindRepresentative(_nodes[a]);
        var bHead = FindRepresentative(_nodes[b]);
        if (aHead != bHead)
        {
            var aSetSize = _sizeMap[aHead];
            var bSetSize = _sizeMap[bHead];
            var big = aSetSize >= bSetSize ? aHead : bHead;
            var small = big == aHead ? bHead : aHead;
            _representatives[small] = big;
            _sizeMap[big] = aSetSize + bSetSize;
            _sizeMap.Remove(small);
        }
    }

    public int Sets()
    {
        return _sizeMap.Count;
    }
}

public static class UnionFindTest
{
    public static void Run()
    {
        var uf = new UnionFind<int>(new List<int> { 1, 2, 3, 4, 5, 6, 7 });
        uf.Union(1, 2);
        uf.Union(2, 3);
        uf.Union(4, 5);
        uf.Union(5, 6);
        uf.Union(6, 7);
        Console.WriteLine(uf.IsSameSet(1, 6)); // true
        Console.WriteLine(uf.IsSameSet(3, 7)); // false
        uf.Union(3, 7);
        Console.WriteLine(uf.IsSameSet(3, 7)); // true
    }
}