#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson37;

public class AddRemoveGetIndexGreat
{
    // 通过以下这个测试，
    // 可以很明显的看到LinkedList的插入、删除、get效率不如SbtList
    // LinkedList需要找到index所在的位置之后才能插入或者读取，时间复杂度O(N)
    // SbtList是平衡搜索二叉树，所以插入或者读取时间复杂度都是O(logN)
    public static void Run()
    {
        // 功能测试
        var test = 50000;
        var max = 1000000;
        var pass = true;
        var list = new List<int>();
        var sbtList = new SbtList<int>();
        for (var i = 0; i < test; i++)
        {
            if (list.Count != sbtList.Size())
            {
                pass = false;
                break;
            }

            if (list.Count > 1 && Utility.GetRandomDouble < 0.5)
            {
                var removeIndex = (int)(Utility.GetRandomDouble * list.Count);
                list.RemoveAt(removeIndex);
                sbtList.Remove(removeIndex);
            }
            else
            {
                var randomIndex = (int)(Utility.GetRandomDouble * (list.Count + 1));
                var randomValue = (int)(Utility.GetRandomDouble * (max + 1));
                list.Insert(randomIndex, randomValue);
                sbtList.Add(randomIndex, randomValue);
            }
        }

        for (var i = 0; i < list.Count; i++)
            if (!list[i].Equals(sbtList.Get(i)))
            {
                pass = false;
                break;
            }

        Console.WriteLine("功能测试是否通过 : " + pass);
        // 性能测试
        test = 500000;
        list = new List<int>();
        sbtList = new SbtList<int>();

        Utility.RestartStopwatch();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.GetRandomDouble * (list.Count + 1));
            var randomValue = (int)(Utility.GetRandomDouble * (max + 1));
            list.Insert(randomIndex, randomValue);
        }

        Console.WriteLine($"List插入总时长{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.GetRandomDouble * (i + 1));
            var temp = list[randomIndex];
        }

        Console.WriteLine($"List读取总时长{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.GetRandomDouble * list.Count);
            list.RemoveAt(randomIndex);
        }

        Console.WriteLine($"List删除总时长{Utility.GetStopwatchElapsedMilliseconds()}ms");

        Utility.RestartStopwatch();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.GetRandomDouble * (sbtList.Size() + 1));
            var randomValue = (int)(Utility.GetRandomDouble * (max + 1));
            sbtList.Add(randomIndex, randomValue);
        }

        Console.WriteLine($"SbtList插入总时长{Utility.GetStopwatchElapsedMilliseconds()}ms");


        Utility.RestartStopwatch();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.GetRandomDouble * (i + 1));
            sbtList.Get(randomIndex);
        }

        Console.WriteLine($"SbtList读取总时长{Utility.GetStopwatchElapsedMilliseconds()}ms");


        Utility.RestartStopwatch();
        for (var i = 0; i < test; i++)
        {
            var randomIndex = (int)(Utility.GetRandomDouble * sbtList.Size());
            sbtList.Remove(randomIndex);
        }

        Console.WriteLine($"SbtList删除总时长{Utility.GetStopwatchElapsedMilliseconds()}ms");
    }
}

public class SbtNode<TV>
{
    public readonly TV Value;
    public SbtNode<TV>? L;
    public SbtNode<TV>? R;
    public int Size;

    public SbtNode(TV v)
    {
        Value = v;
        Size = 1;
    }
}

public class SbtList<TV>
{
    private SbtNode<TV>? _root;

    //右旋
    protected virtual SbtNode<TV> RightRotate(SbtNode<TV> cur)
    {
        var leftNode = cur.L;
        if (leftNode != null)
        {
            cur.L = leftNode.R;
            leftNode.R = cur;
            leftNode.Size = cur.Size;
            cur.Size = (cur.L?.Size ?? 0) + (cur.R?.Size ?? 0) + 1;
            return leftNode;
        }

        throw new InvalidOperationException();
    }

    //左旋
    protected virtual SbtNode<TV> LeftRotate(SbtNode<TV> cur)
    {
        var rightNode = cur.R;
        if (rightNode != null)
        {
            cur.R = rightNode.L;
            rightNode.L = cur;
            rightNode.Size = cur.Size;
            cur.Size = (cur.L?.Size ?? 0) + (cur.R?.Size ?? 0) + 1;
            return rightNode;
        }

        throw new InvalidOperationException();
    }

    protected virtual SbtNode<TV>? Maintain(SbtNode<TV>? cur)
    {
        if (cur == null) return null;

        var leftSize = cur.L?.Size ?? 0;
        var leftLeftSize = cur.L is { L: not null } ? cur.L.L.Size : 0;
        var leftRightSize = cur.L is { R: not null } ? cur.L.R.Size : 0;
        var rightSize = cur.R?.Size ?? 0;
        var rightLeftSize = cur.R is { L: not null } ? cur.R.L.Size : 0;
        var rightRightSize = cur.R is { R: not null } ? cur.R.R.Size : 0;
        if (leftLeftSize > rightSize)
        {
            cur = RightRotate(cur);
            cur.R = Maintain(cur.R);
            cur = Maintain(cur);
        }
        else if (leftRightSize > rightSize)
        {
            if (cur.L != null)
            {
                cur.L = LeftRotate(cur.L);
                cur = RightRotate(cur);
                cur.L = Maintain(cur.L);
            }

            cur.R = Maintain(cur.R);
            cur = Maintain(cur);
        }
        else if (rightRightSize > leftSize)
        {
            cur = LeftRotate(cur);
            cur.L = Maintain(cur.L);
            cur = Maintain(cur);
        }
        else if (rightLeftSize > leftSize)
        {
            if (cur.R != null)
            {
                cur.R = RightRotate(cur.R);
                cur = LeftRotate(cur);
                cur.L = Maintain(cur.L);
                cur.R = Maintain(cur.R);
            }

            cur = Maintain(cur);
        }

        return cur;
    }

    protected virtual SbtNode<TV>? Add(SbtNode<TV>? root, int index, SbtNode<TV> cur)
    {
        if (root == null) return cur;

        root.Size++;
        var leftAndHeadSize = (root.L?.Size ?? 0) + 1;
        if (index < leftAndHeadSize)
            root.L = Add(root.L, index, cur);
        else
            root.R = Add(root.R, index - leftAndHeadSize, cur);

        root = Maintain(root);
        return root;
    }

    protected virtual SbtNode<TV>? Remove(SbtNode<TV> root, int index)
    {
        root.Size--;
        var rootIndex = root.L?.Size ?? 0;
        if (index != rootIndex)
        {
            if (index < rootIndex)
            {
                if (root.L != null) root.L = Remove(root.L, index);
            }
            else
            {
                if (root.R != null) root.R = Remove(root.R, index - rootIndex - 1);
            }

            return root;
        }

        if (root.L == null && root.R == null) return null;

        if (root.L == null) return root.R;

        if (root.R == null) return root.L;

        SbtNode<TV>? pre = null;
        var suc = root.R;
        suc.Size--;
        while (suc.L != null)
        {
            pre = suc;
            suc = suc.L;
            suc.Size--;
        }

        if (pre != null)
        {
            pre.L = suc.R;
            suc.R = root.R;
        }

        suc.L = root.L;
        suc.Size = suc.L.Size + (suc.R?.Size ?? 0) + 1;
        return suc;
    }

    protected virtual SbtNode<TV> Get(SbtNode<TV> root, int index)
    {
        var leftSize = root.L?.Size ?? 0;
        if (index < leftSize)
            if (root.L != null)
                return Get(root.L, index);
        if (index == leftSize)
            return root;
        if (root.R != null) return Get(root.R, index - leftSize - 1);
        throw new InvalidOperationException();
    }

    public virtual void Add(int index, TV num)
    {
        var cur = new SbtNode<TV>(num);
        if (_root == null)
        {
            _root = cur;
        }
        else
        {
            if (index <= _root.Size) _root = Add(_root, index, cur);
        }
    }

    public virtual TV Get(int index)
    {
        if (_root == null) throw new InvalidOperationException();
        var ans = Get(_root, index);
        return ans.Value;
    }

    public virtual void Remove(int index)
    {
        if (index >= 0 && Size() > index)
            if (_root != null)
                _root = Remove(_root, index);
            else
                throw new InvalidOperationException();
    }

    public virtual int Size()
    {
        return _root?.Size ?? 0;
    }
}