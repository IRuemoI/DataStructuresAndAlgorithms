#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson36;

// 跳表的节点定义
public class SkipListNode<TK, TV> where TK : IComparable<TK>, IEquatable<TK>
{
    public readonly List<SkipListNode<TK, TV>?> NextNodes;
    public TK Key;
    public TV Value;


    public SkipListNode(TK k, TV v)
    {
        Key = k;
        Value = v;
        NextNodes = new List<SkipListNode<TK, TV>?>();
    }

    // 遍历的时候，如果是往右遍历到的null(next == null), 遍历结束
    // 头(null), 头节点的null，认为最小
    // node  -> 头，node(null, "")  node.isKeyLess(!null)  true
    // node里面的key是否比otherKey小，true，不是false
    public virtual bool IsKeyLess(TK? otherKey)
    {
        //  otherKey == null -> false 
        return otherKey != null && (Key.Equals(default) || Key.CompareTo(otherKey) < 0);
    }

    public virtual bool IsKeyEqual(TK? otherKey)
    {
        return (Key == null && otherKey == null)
               || (
                   Key != null && otherKey != null &&
                   Key.CompareTo(otherKey) == 0
               );
    }
}

public class SkipListMap<TK, TV> where TK : IComparable<TK>, IEquatable<TK>
{
    private const double Probability = 0.5; // < 0.5 继续做，>=0.5 停
    private readonly SkipListNode<TK, TV>? _head = new(default!, default!);
    private int _maxLevel;

    public SkipListMap()
    {
        _head?.NextNodes.Add(null); // 0
        Size = 0;
        _maxLevel = 0;
    }

    public int Size { private set; get; }

    // 从最高层开始，一路找下去，
    // 最终，找到第0层的<key的最右的节点
    protected virtual SkipListNode<TK, TV>? MostRightLessNodeInTree(TK key)
    {
        if (key.Equals(default)) return null;

        var level = _maxLevel;
        var cur = _head;
        while (level >= 0)
            // 从上层跳下层
            //  cur  level  -> level-1
            cur = MostRightLessNodeInLevel(key, cur, level--);

        return cur;
    }

    // 在level层里，如何往右移动
    // 现在来到的节点是cur，来到了cur的level层，在level层上，找到<key最后一个节点并返回
    protected virtual SkipListNode<TK, TV>? MostRightLessNodeInLevel(TK key, SkipListNode<TK, TV>? cur, int level)
    {
        var next = cur?.NextNodes[level];
        while (next != null && next.IsKeyLess(key))
        {
            cur = next;
            next = cur.NextNodes[level];
        }

        return cur;
    }

    public virtual bool ContainsKey(TK key)
    {
        if (key.Equals(default)) return false;

        var less = MostRightLessNodeInTree(key);
        var next = less?.NextNodes[0];
        return next != null && next.IsKeyEqual(key);
    }

    // 新增、改value
    public virtual void Put(TK key, TV value)
    {
        if (key.Equals(default)) return;

        // 0层上，最右一个，< key 的Node -> >key
        var less = MostRightLessNodeInTree(key);
        var find = less?.NextNodes[0];
        if (find != null && find.IsKeyEqual(key))
        {
            find.Value = value;
        }
        else
        {
            // find == null   8   7   9
            Size++;
            var newNodeLevel = 0;
            while (Utility.getRandomDouble < Probability) newNodeLevel++;

            // newNodeLevel
            while (newNodeLevel > _maxLevel)
            {
                _head?.NextNodes.Add(null);
                _maxLevel++;
            }

            var newNode = new SkipListNode<TK, TV>(key, value);
            for (var i = 0; i <= newNodeLevel; i++) newNode.NextNodes.Add(null);

            var level = _maxLevel;
            var pre = _head;
            while (level >= 0)
            {
                // level 层中，找到最右的 < key 的节点
                pre = MostRightLessNodeInLevel(key, pre, level);
                if (level <= newNodeLevel)
                    if (pre != null)
                    {
                        newNode.NextNodes[level] = pre.NextNodes[level];
                        pre.NextNodes[level] = newNode;
                    }

                level--;
            }
        }
    }

    public virtual TV? Get(TK key)
    {
        if (key.Equals(default)) return default;

        var less = MostRightLessNodeInTree(key);
        var next = less?.NextNodes[0];
        return next != null && next.IsKeyEqual(key) ? next.Value : default;
    }

    public virtual void Remove(TK key)
    {
        if (ContainsKey(key))
        {
            Size--;
            var level = _maxLevel;
            var pre = _head;
            while (level >= 0)
            {
                pre = MostRightLessNodeInLevel(key, pre, level);
                if (pre != null)
                {
                    var next = pre.NextNodes[level];
                    // 1）在这一层中，pre下一个就是key
                    // 2）在这一层中，pre的下一个key是>要删除key
                    if (next != null && next.IsKeyEqual(key))
                        // level : pre -> next(key) -> ...
                        pre.NextNodes[level] = next.NextNodes[level];
                }

                // 在level层只有一个节点了，就是默认节点head
                if (pre != null && level != 0 && pre == _head && pre.NextNodes[level] == null)
                {
                    _head.NextNodes.RemoveAt(level);
                    _maxLevel--;
                }

                level--;
            }
        }
    }

    public virtual TK? FirstKey()
    {
        return _head?.NextNodes[0] != null ? _head.NextNodes[0]!.Key : default;
    }

    public virtual TK? LastKey()
    {
        var level = _maxLevel;
        var cur = _head;
        while (level >= 0)
        {
            var next = cur?.NextNodes[level];
            while (next != null)
            {
                cur = next;
                next = cur.NextNodes[level];
            }

            level--;
        }

        return cur != null ? cur.Key : default;
    }

    public virtual TK? CeilingKey(TK key)
    {
        if (key.Equals(default)) return default;

        var less = MostRightLessNodeInTree(key);
        var next = less?.NextNodes[0];
        return next != null ? next.Key : default;
    }

    public virtual TK? FloorKey(TK key)
    {
        if (key.Equals(default)) return default;

        var less = MostRightLessNodeInTree(key);
        var next = less?.NextNodes[0];
        return next != null && next.IsKeyEqual(key) ? next.Key : less!.Key;
    }


    //用于测试
    public static void PrintAll(SkipListMap<string, string> obj)
    {
        for (var i = obj._maxLevel; i >= 0; i--)
        {
            Console.Write("Level " + i + " : ");
            var cur = obj._head;
            while (cur?.NextNodes[i] != null)
            {
                var next = cur.NextNodes[i];
                Console.Write("(" + next?.Key + " , " + next?.Value + ") ");
                cur = next;
            }

            Console.WriteLine();
        }
    }
}

public class SkipListMapTest
{
    public static void Run()
    {
        var test = new SkipListMap<string, string>();
        SkipListMap<string, string>.PrintAll(test);
        Console.WriteLine("======================");
        test.Put("A", "10");
        SkipListMap<string, string>.PrintAll(test);
        Console.WriteLine("======================");
        test.Remove("A");
        SkipListMap<string, string>.PrintAll(test);
        Console.WriteLine("======================");
        test.Put("E", "E");
        test.Put("B", "B");
        test.Put("A", "A");
        test.Put("F", "F");
        test.Put("C", "C");
        test.Put("D", "D");
        SkipListMap<string, string>.PrintAll(test);
        Console.WriteLine("======================");
        Console.WriteLine(test.ContainsKey("B"));
        Console.WriteLine(test.ContainsKey("Z"));
        Console.WriteLine("FirstKey:" + test.FirstKey());
        Console.WriteLine("LastKey:" + test.LastKey());
        Console.WriteLine(test.FloorKey("D"));
        Console.WriteLine(test.CeilingKey("D"));
        Console.WriteLine("======================");
        test.Remove("D");
        SkipListMap<string, string>.PrintAll(test);
        Console.WriteLine("======================");
        Console.WriteLine(test.FloorKey("D"));
        Console.WriteLine(test.CeilingKey("D"));
    }
}