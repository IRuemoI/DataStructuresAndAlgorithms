//测试通过

namespace Common.DataStructures.UnionFindSet;

public class Node<T>(T v)
{
    public T GetValue()
    {
        return v;
    }
}

/// <summary>
///     用于实际访问的实际内容的并查集
/// </summary>
/// <typeparam name="T">泛型</typeparam>
public class GenericUnionFindSet<T> where T : notnull
{
    //每个元素与包装对象的对应关系
    private readonly Dictionary<T, Node<T>> _nodes;

    //包装对象与集合代表对象的对应关系
    private readonly Dictionary<Node<T>, Node<T>> _representatives;

    //只含有每个集合的代表元素与所在集合的大小的对应关系
    private readonly Dictionary<Node<T>, int> _sizeMap;

    public GenericUnionFindSet(List<T> values)
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

/// <summary>
///     用于只关心元素是否在同一个集合中的并查集
/// </summary>
public class UnionFindSet
{
    // 辅助结构
    private readonly int[] _help;

    // size[i] = k ： 如果i是代表节点，size[i]才有意义，否则无意义
    // i所在的集合大小是多少
    private readonly int[] _perSetSize;

    // parent[i] = k ： i的父亲是k
    private readonly int[] _representative;

    // 一共有多少个集合
    private int _setCount;

    public UnionFindSet(int initElementCount)
    {
        _representative = new int[initElementCount];
        _perSetSize = new int[initElementCount];
        _help = new int[initElementCount];
        _setCount = initElementCount;
        for (var i = 0; i < initElementCount; i++)
        {
            _representative[i] = i;
            _perSetSize[i] = 1;
        }
    }

    // 从i开始一直往上，往上到不能再往上，代表节点，返回
    // 这个过程要做路径压缩
    private int FindRepresentative(int i)
    {
        var representativeCursorOfI = 0;
        while (i != _representative[i])
        {
            _help[representativeCursorOfI++] = i;
            i = _representative[i];
        }

        for (representativeCursorOfI--; representativeCursorOfI >= 0; representativeCursorOfI--)
            _representative[_help[representativeCursorOfI]] = i;

        return i;
    }

    /// <summary>
    ///     查找两个元素是否处于同一个集合
    /// </summary>
    /// <param name="i">元素i的数组下标</param>
    /// <param name="j">元素j的数组下标</param>
    public bool IsSameSet(int i, int j)
    {
        return FindRepresentative(i) == FindRepresentative(j);
    }

    /// <summary>
    ///     让两个元素所在的集合合并为同一个集合
    /// </summary>
    /// <param name="i">元素i的数组下标</param>
    /// <param name="j">元素j的数组下标</param>
    public void Union(int i, int j)
    {
        var f1 = FindRepresentative(i);
        var f2 = FindRepresentative(j);
        if (f1 != f2)
        {
            if (_perSetSize[f1] >= _perSetSize[f2])
            {
                _perSetSize[f1] += _perSetSize[f2];
                _representative[f2] = f1;
            }
            else
            {
                _perSetSize[f2] += _perSetSize[f1];
                _representative[f1] = f2;
            }

            _setCount--;
        }
    }

    public int SetCount()
    {
        return _setCount;
    }
}

public static class UnionFindTest
{
    public static void Run()
    {
        var uf = new GenericUnionFindSet<int>([1, 2, 3, 4, 5, 6, 7]);
        uf.Union(1, 2);
        uf.Union(2, 3);
        uf.Union(4, 5);
        uf.Union(5, 6);
        uf.Union(6, 7);
        Console.WriteLine(uf.IsSameSet(1, 6)); // true
        Console.WriteLine(uf.IsSameSet(3, 7)); // false
        uf.Union(3, 7);
        Console.WriteLine(uf.IsSameSet(3, 7)); // true

        Console.WriteLine("-----------------------");

        var uf1 = new UnionFindSet(7);
        uf1.Union(1 - 1, 2 - 1);
        uf1.Union(2 - 1, 3 - 1);
        uf1.Union(4 - 1, 5 - 1);
        uf1.Union(5 - 1, 6 - 1);
        uf1.Union(6 - 1, 7 - 1);
        Console.WriteLine(uf1.IsSameSet(1 - 1, 6 - 1)); // true
        Console.WriteLine(uf1.IsSameSet(3 - 1, 7 - 1)); // false
        uf1.Union(3 - 1, 7 - 1);
        Console.WriteLine(uf1.IsSameSet(3 - 1, 7 - 1)); // true
        Console.WriteLine(uf1.SetCount());
    }
}