#region

using System.Text;

#endregion

namespace Algorithms.Lesson36;

public class SizeBalancedTreeMapTest
{
    //用于测试
    private static void PrintAll(SbtNode<string, int>? head)
    {
        Console.WriteLine("Binary Tree:");
        PrintInOrder(head, 0, "H", 17);
        Console.WriteLine();
    }

    //用于测试
    private static void PrintInOrder(SbtNode<string, int>? head, int height, string to, int len)
    {
        if (head == null) return;

        PrintInOrder(head.RightChild, height + 1, "v", len);
        var val = to + "(" + head.Key + "," + head.Value + ")" + to;
        var lenM = val.Length;
        var lenL = (len - lenM) / 2;
        var lenR = len - lenM - lenL;
        val = GetSpace(lenL) + val + GetSpace(lenR);
        Console.WriteLine(GetSpace(height * len) + val);
        PrintInOrder(head.LeftChild, height + 1, "^", len);
    }

    //用于测试
    private static string GetSpace(int num)
    {
        var space = " ";
        var buf = new StringBuilder("");
        for (var i = 0; i < num; i++) buf.Append(space);

        return buf.ToString();
    }

    public static void Run()
    {
        var sbt = new SizeBalancedTreeMap<string, int>();
        sbt.Put("d", 4);
        sbt.Put("c", 3);
        sbt.Put("a", 1);
        sbt.Put("b", 2);
        sbt.Put("e", 5);
        sbt.Put("g", 7);
        sbt.Put("f", 6);
        sbt.Put("h", 8);
        sbt.Put("i", 9);
        sbt.Put("a", 111);
        Console.WriteLine(sbt.Get("a"));
        sbt.Put("a", 1);
        Console.WriteLine("size:" + sbt.Size);
        Console.WriteLine(sbt.Get("a"));
        for (var i = 0; i < sbt.Size; i++) Console.WriteLine(sbt.GetIndexKey(i) + " , " + sbt.GetIndexValue(i));

        PrintAll(sbt.Root);
        Console.WriteLine(sbt.FirstKey());
        Console.WriteLine(sbt.LastKey());
        Console.WriteLine(sbt.FloorKey("g"));
        Console.WriteLine(sbt.CeilingKey("g"));
        Console.WriteLine(sbt.FloorKey("e"));
        Console.WriteLine(sbt.CeilingKey("e"));
        Console.WriteLine(sbt.FloorKey(""));
        Console.WriteLine(sbt.CeilingKey(""));
        Console.WriteLine(sbt.FloorKey("j"));
        Console.WriteLine(sbt.CeilingKey("j"));
        sbt.Remove("d");
        PrintAll(sbt.Root);
        sbt.Remove("f");
        PrintAll(sbt.Root);
    }
}

public class SbtNode<TK, TV> where TK : IComparable<TK>, IEquatable<TK>
{
    public readonly TK Key;
    public SbtNode<TK, TV>? LeftChild;
    public SbtNode<TK, TV>? RightChild;
    public int Size; // 不同的key的数量
    public TV Value;

    public SbtNode(TK key, TV value)
    {
        Key = key;
        Value = value;
        Size = 1;
    }
}

public class SizeBalancedTreeMap<TK, TV> where TK : IComparable<TK>, IEquatable<TK>
{
    public SbtNode<TK, TV>? Root;
    public int Size => Root?.Size ?? 0;

    protected virtual SbtNode<TK, TV> RightRotate(SbtNode<TK, TV>? cur)
    {
        if (cur != null)
        {
            var leftNode = cur.LeftChild;
            if (leftNode != null)
            {
                cur.LeftChild = leftNode.RightChild;
                leftNode.RightChild = cur;
                leftNode.Size = cur.Size;
                cur.Size = (cur.LeftChild?.Size ?? 0) + (cur.RightChild?.Size ?? 0) + 1;
                return leftNode;
            }
        }

        throw new InvalidOperationException();
    }

    protected virtual SbtNode<TK, TV> LeftRotate(SbtNode<TK, TV>? cur)
    {
        if (cur != null)
        {
            var rightNode = cur.RightChild;
            if (rightNode != null)
            {
                cur.RightChild = rightNode.LeftChild;
                rightNode.LeftChild = cur;
                rightNode.Size = cur.Size;
                cur.Size = (cur.LeftChild?.Size ?? 0) + (cur.RightChild?.Size ?? 0) + 1;
                return rightNode;
            }
        }

        throw new InvalidOperationException();
    }

    protected virtual SbtNode<TK, TV>? Maintain(SbtNode<TK, TV>? cur)
    {
        if (cur == null) return null;
        var leftSize = cur.LeftChild?.Size ?? 0;
        var leftLeftSize = cur.LeftChild is { LeftChild: not null } ? cur.LeftChild.LeftChild.Size : 0;
        var leftRightSize = cur.LeftChild is { RightChild: not null } ? cur.LeftChild.RightChild.Size : 0;
        var rightSize = cur.RightChild?.Size ?? 0;
        var rightLeftSize = cur.RightChild is { LeftChild: not null } ? cur.RightChild.LeftChild.Size : 0;
        var rightRightSize = cur.RightChild is { RightChild: not null } ? cur.RightChild.RightChild.Size : 0;
        if (leftLeftSize > rightSize)
        {
            cur = RightRotate(cur);
            cur.RightChild = Maintain(cur.RightChild);
            cur = Maintain(cur);
        }
        else if (leftRightSize > rightSize)
        {
            cur.LeftChild = LeftRotate(cur.LeftChild);
            cur = RightRotate(cur);
            cur.LeftChild = Maintain(cur.LeftChild);
            cur.RightChild = Maintain(cur.RightChild);
            cur = Maintain(cur);
        }
        else if (rightRightSize > leftSize)
        {
            cur = LeftRotate(cur);
            cur.LeftChild = Maintain(cur.LeftChild);
            cur = Maintain(cur);
        }
        else if (rightLeftSize > leftSize)
        {
            cur.RightChild = RightRotate(cur.RightChild);
            cur = LeftRotate(cur);
            cur.LeftChild = Maintain(cur.LeftChild);
            cur.RightChild = Maintain(cur.RightChild);
            cur = Maintain(cur);
        }

        return cur;
    }

    protected virtual SbtNode<TK, TV>? FindLastIndex(TK key)
    {
        var pre = Root;
        var cur = Root;
        while (cur != null)
        {
            pre = cur;
            if (key.CompareTo(cur.Key) == 0)
                break;
            cur = key.CompareTo(cur.Key) < 0 ? cur.LeftChild : cur.RightChild;
        }

        return pre;
    }

    protected virtual SbtNode<TK, TV>? FindLastNoSmallIndex(TK key)
    {
        SbtNode<TK, TV>? ans = null;
        var cur = Root;
        while (cur != null)
            if (key.CompareTo(cur.Key) == 0)
            {
                ans = cur;
                break;
            }
            else if (key.CompareTo(cur.Key) < 0)
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

    protected virtual SbtNode<TK, TV>? FindLastNoBigIndex(TK key)
    {
        SbtNode<TK, TV>? ans = null;
        var cur = Root;
        while (cur != null)
            if (key.CompareTo(cur.Key) == 0)
            {
                ans = cur;
                break;
            }
            else if (key.CompareTo(cur.Key) < 0)
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

    // 现在，以cur为头的树上，新增，加(key, value)这样的记录
    // 加完之后，会对cur做检查，该调整调整
    // 返回，调整完之后，整棵树的新头部
    protected virtual SbtNode<TK, TV>? Add(SbtNode<TK, TV>? cur, TK key, TV value)
    {
        if (cur == null) return new SbtNode<TK, TV>(key, value);

        cur.Size++;
        if (key.CompareTo(cur.Key) < 0)
            cur.LeftChild = Add(cur.LeftChild, key, value);
        else
            cur.RightChild = Add(cur.RightChild, key, value);

        return Maintain(cur);
    }

    // 在cur这棵树上，删掉key所代表的节点
    // 返回cur这棵树的新头部
    protected virtual SbtNode<TK, TV>? Delete(SbtNode<TK, TV>? cur, TK key)
    {
        if (cur != null)
        {
            cur.Size--;
            if (key.CompareTo(cur.Key) > 0)
            {
                cur.RightChild = Delete(cur.RightChild, key);
            }
            else if (key.CompareTo(cur.Key) < 0)
            {
                cur.LeftChild = Delete(cur.LeftChild, key);
            }
            else
            {
                // 当前要删掉cur
                if (cur.LeftChild == null && cur.RightChild == null)
                {
                    // free cur memory -> C++
                    cur = null;
                }
                else if (cur.LeftChild == null && cur.RightChild != null)
                {
                    // free cur memory -> C++
                    cur = cur.RightChild;
                }
                else if (cur is { LeftChild: not null, RightChild: null })
                {
                    // free cur memory -> C++
                    cur = cur.LeftChild;
                }
                else
                {
                    // 有左有右
                    SbtNode<TK, TV>? pre = null;
                    var des = cur.RightChild;
                    if (des != null)
                    {
                        des.Size--;
                        while (des.LeftChild != null)
                        {
                            pre = des;
                            des = des.LeftChild;
                            des.Size--;
                        }

                        if (pre != null)
                        {
                            pre.LeftChild = des.RightChild;
                            des.RightChild = cur.RightChild;
                        }

                        des.LeftChild = cur.LeftChild;
                        if (des.LeftChild != null) des.Size = des.LeftChild.Size + (des.RightChild?.Size ?? 0) + 1;
                        // free cur memory -> C++
                        cur = des;
                    }
                }
            }

            //cur = Maintain(cur);
            return cur;
        }

        throw new InvalidOperationException();
    }

    protected virtual SbtNode<TK, TV> GetIndex(SbtNode<TK, TV>? cur, int kth)
    {
        if (cur != null)
        {
            if (kth == (cur.LeftChild?.Size ?? 0) + 1) return cur;
            return kth <= (cur.LeftChild?.Size ?? 0)
                ? GetIndex(cur.LeftChild, kth)
                : GetIndex(cur.RightChild, kth - (cur.LeftChild?.Size ?? 0) - 1);
        }

        throw new InvalidOperationException();
    }

    public virtual bool ContainsKey(TK? key)
    {
        if (key == null) throw new Exception("invalid parameter.");

        var lastNode = FindLastIndex(key);
        return lastNode != null && key.CompareTo(lastNode.Key) == 0;
    }

    // （key，value） put -> 有序表 新增、改value
    public virtual void Put(TK? key, TV value)
    {
        if (key == null) throw new Exception("invalid parameter.");

        var lastNode = FindLastIndex(key);
        //如果最后一个节点不为空且等于当前键
        if (lastNode != null && key.CompareTo(lastNode.Key) == 0)
            lastNode.Value = value; //更新值
        else
            Root = Add(Root, key, value); //添加值
    }

    public virtual void Remove(TK? key)
    {
        if (key == null) throw new Exception("invalid parameter.");

        if (ContainsKey(key)) Root = Delete(Root, key);
    }

    public virtual TK GetIndexKey(int index)
    {
        if (index < 0 || index >= Size) throw new Exception("invalid parameter.");

        return GetIndex(Root, index + 1).Key;
    }

    public virtual TV GetIndexValue(int index)
    {
        if (index < 0 || index >= Size) throw new Exception("invalid parameter.");

        return GetIndex(Root, index + 1).Value;
    }

    public virtual TV? Get(TK? key)
    {
        if (key == null) throw new Exception("invalid parameter.");

        var lastNode = FindLastIndex(key);
        if (lastNode != null && key.CompareTo(lastNode.Key) == 0)
            return lastNode.Value;
        return default;
    }

    public virtual TK? FirstKey()
    {
        if (Root == null) return default;

        var cur = Root;
        while (cur.LeftChild != null) cur = cur.LeftChild;

        return cur.Key;
    }

    public virtual TK? LastKey()
    {
        if (Root == null) return default;

        var cur = Root;
        while (cur.RightChild != null) cur = cur.RightChild;

        return cur.Key;
    }

    public virtual TK? FloorKey(TK key)
    {
        if (key.Equals(default)) throw new Exception("invalid parameter.");

        var lastNoBigNode = FindLastNoBigIndex(key);
        return lastNoBigNode == null ? default : lastNoBigNode.Key;
    }

    public virtual TK? CeilingKey(TK key)
    {
        if (key.Equals(default)) throw new Exception("invalid parameter.");

        var lastNoSmallNode = FindLastNoSmallIndex(key);
        return lastNoSmallNode == null ? default : lastNoSmallNode.Key;
    }
}