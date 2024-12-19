namespace Algorithms.Lesson35;

public class AvlTreeMap<TK, TV> where TK : IComparable<TK>, IEquatable<TK>
{
    private AvlNode<TK, TV>? _root;

    public int size { private set; get; }

    private AvlNode<TK, TV> RightRotate(AvlNode<TK, TV> cur)
    {
        var left = cur.LeftChild;
        if (left == null) throw new InvalidOperationException();
        cur.LeftChild = left.RightChild;
        left.RightChild = cur;
        cur.H = Math.Max(cur.LeftChild?.H ?? 0, cur.RightChild?.H ?? 0) + 1;
        left.H = Math.Max(left.LeftChild?.H ?? 0, left.RightChild?.H ?? 0) + 1;
        return left;
    }

    private AvlNode<TK, TV> LeftRotate(AvlNode<TK, TV> cur)
    {
        var right = cur.RightChild;
        if (right == null) throw new InvalidOperationException();
        cur.RightChild = right.LeftChild;
        right.LeftChild = cur;
        cur.H = Math.Max(cur.LeftChild?.H ?? 0, cur.RightChild?.H ?? 0) + 1;
        right.H = Math.Max(right.LeftChild?.H ?? 0, right.RightChild?.H ?? 0) + 1;
        return right;
    }

    private AvlNode<TK, TV>? Maintain(AvlNode<TK, TV>? cur)
    {
        if (cur == null) return null;

        var leftHeight = cur.LeftChild?.H ?? 0;
        var rightHeight = cur.RightChild?.H ?? 0;
        if (Math.Abs(leftHeight - rightHeight) > 1)
        {
            if (leftHeight > rightHeight)
            {
                var leftLeftHeight = cur.LeftChild is { LeftChild: not null } ? cur.LeftChild.LeftChild.H : 0;
                var leftRightHeight = cur.LeftChild is { RightChild: not null } ? cur.LeftChild.RightChild.H : 0;
                if (leftLeftHeight >= leftRightHeight)
                {
                    cur = RightRotate(cur);
                }
                else
                {
                    cur.LeftChild = LeftRotate(cur.LeftChild ?? throw new InvalidOperationException());
                    cur = RightRotate(cur);
                }
            }
            else
            {
                var rightLeftHeight = cur.RightChild is { LeftChild: not null } ? cur.RightChild.LeftChild.H : 0;
                var rightRightHeight = cur.RightChild is { RightChild: not null } ? cur.RightChild.RightChild.H : 0;
                if (rightRightHeight >= rightLeftHeight)
                {
                    cur = LeftRotate(cur);
                }
                else
                {
                    cur.RightChild = RightRotate(cur.RightChild ?? throw new InvalidOperationException());
                    cur = LeftRotate(cur);
                }
            }
        }

        return cur;
    }

    private AvlNode<TK, TV>? FindLastIndex(TK key)
    {
        var pre = _root;
        var cur = _root;
        while (cur != null)
        {
            pre = cur;
            if (key.CompareTo(cur.K) == 0)
                break;
            cur = key.CompareTo(cur.K) < 0 ? cur.LeftChild : cur.RightChild;
        }

        return pre;
    }

    private AvlNode<TK, TV>? FindLastNoSmallIndex(TK key)
    {
        AvlNode<TK, TV>? ans = null;
        var cur = _root;
        while (cur != null)
            if (key.CompareTo(cur.K) == 0)
            {
                ans = cur;
                break;
            }
            else if (key.CompareTo(cur.K) < 0)
            {
                ans = cur;
                cur = cur.LeftChild;
            }
            else
            {
                cur = cur.RightChild;
            }

        return ans;
    }

    private AvlNode<TK, TV>? FindLastNoBigIndex(TK key)
    {
        AvlNode<TK, TV>? ans = null;
        var cur = _root;
        while (cur != null)
            if (key.CompareTo(cur.K) == 0)
            {
                ans = cur;
                break;
            }
            else if (key.CompareTo(cur.K) < 0)
            {
                cur = cur.LeftChild;
            }
            else
            {
                ans = cur;
                cur = cur.RightChild;
            }

        return ans;
    }

    private AvlNode<TK, TV>? Add(AvlNode<TK, TV>? cur, TK key, TV value)
    {
        if (cur == null) return new AvlNode<TK, TV>(key, value);

        if (key.CompareTo(cur.K) < 0)
            cur.LeftChild = Add(cur.LeftChild, key, value);
        else
            cur.RightChild = Add(cur.RightChild, key, value);

        cur.H = Math.Max(cur.LeftChild?.H ?? 0, cur.RightChild?.H ?? 0) + 1;
        return Maintain(cur);
    }

    // 在cur这棵树上，删掉key所代表的节点
    // 返回cur这棵树的新头部
    private AvlNode<TK, TV>? Delete(AvlNode<TK, TV>? cur, TK? key)
    {
        if (key != null && cur != null && key.CompareTo(cur.K) > 0)
        {
            if (cur.RightChild != null) cur.RightChild = Delete(cur.RightChild, key);
        }
        else if (cur != null && key != null && key.CompareTo(cur.K) < 0)
        {
            if (cur.LeftChild != null) cur.LeftChild = Delete(cur.LeftChild, key);
        }
        else
        {
            if (cur?.LeftChild == null && cur?.RightChild == null)
            {
                cur = null;
            }
            else if (cur.LeftChild == null && cur.RightChild != null)
            {
                cur = cur.RightChild;
            }
            else if (cur is { LeftChild: not null, RightChild: null })
            {
                cur = cur.LeftChild;
            }
            else
            {
                var des = cur.RightChild;
                while (des?.LeftChild != null) des = des.LeftChild;

                if (des != null)
                {
                    cur.RightChild = Delete(cur.RightChild, des.K);
                    des.LeftChild = cur.LeftChild;
                    des.RightChild = cur.RightChild;
                    cur = des;
                }
            }
        }

        if (cur != null) cur.H = Math.Max(cur.LeftChild?.H ?? 0, cur.RightChild?.H ?? 0) + 1;

        return Maintain(cur);
    }

    public bool ContainsKey(TK key)
    {
        if (key.Equals(default)) return false;

        var lastNode = FindLastIndex(key);
        return lastNode != null && key.CompareTo(lastNode.K) == 0;
    }

    public void Put(TK key, TV value)
    {
        if (key.Equals(default)) return;

        var lastNode = FindLastIndex(key);
        if (lastNode != null && key.CompareTo(lastNode.K) == 0)
        {
            lastNode.V = value;
        }
        else
        {
            size++;
            _root = Add(_root, key, value);
        }
    }

    public void Remove(TK key)
    {
        if (key.Equals(default)) return;

        if (ContainsKey(key))
        {
            size--;
            _root = Delete(_root, key);
        }
    }

    public TV? Get(TK key)
    {
        if (key.Equals(default)) return default;

        var lastNode = FindLastIndex(key);
        if (lastNode != null && key.CompareTo(lastNode.K) == 0) return lastNode.V;

        return default;
    }


    public TK? FirstKey()
    {
        if (_root == null) return default;

        var cur = _root;
        while (cur.LeftChild != null) cur = cur.LeftChild;

        return cur.K;
    }

    public TK? LastKey()
    {
        if (_root == null) return default;

        var cur = _root;
        while (cur.RightChild != null) cur = cur.RightChild;

        return cur.K;
    }

    public TK? FloorKey(TK key)
    {
        if (key.Equals(default)) return default;

        var lastNoBigNode = FindLastNoBigIndex(key);
        return lastNoBigNode == null ? default : lastNoBigNode.K;
    }

    public TK? CeilingKey(TK key)
    {
        if (key.Equals(default)) return default;

        var lastNoSmallNode = FindLastNoSmallIndex(key);
        return lastNoSmallNode == null ? default : lastNoSmallNode.K;
    }
}

public class AvlTreeMapTest
{
    public static void Run()
    {
        //avl树测试
        var avlTreeMap = new AvlTreeMap<int, string>();
        avlTreeMap.Put(1, "one");
        avlTreeMap.Put(2, "two");
        avlTreeMap.Put(3, "three");
        Console.WriteLine(avlTreeMap.FirstKey());
        Console.WriteLine(avlTreeMap.LastKey());
        Console.WriteLine(avlTreeMap.FloorKey(2));
        Console.WriteLine(avlTreeMap.CeilingKey(1));
    }
}