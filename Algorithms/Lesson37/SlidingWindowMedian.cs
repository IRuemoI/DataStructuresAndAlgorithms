namespace Algorithms.Lesson37;

public static class SlidingWindowMedian
{
    public static double[] MedianSlidingWindow(int[] nums, int k)
    {
        var map = new SizeBalancedTreeMap<Node>();
        for (var i = 0; i < k - 1; i++) map.Add(new Node(i, nums[i]));

        var ans = new double[nums.Length - k + 1];
        var index = 0;
        for (var i = k - 1; i < nums.Length; i++)
        {
            map.Add(new Node(i, nums[i]));
            if (map.Size() % 2 == 0)
            {
                var upMid = map.GetIndexKey(map.Size() / 2 - 1);
                var downMid = map.GetIndexKey(map.Size() / 2);
                ans[index++] = ((double)upMid.Value + downMid.Value) / 2;
            }
            else
            {
                var mid = map.GetIndexKey(map.Size() / 2);
                ans[index++] = mid.Value;
            }

            map.Remove(new Node(i - k + 1, nums[i - k + 1]));
        }

        return ans;
    }

    private class SbtNode<TK>(TK k)
        where TK : IComparable<TK>
    {
        public readonly TK Key = k;
        public SbtNode<TK>? LeftChild;
        public SbtNode<TK>? RightChild;

        public int Size { set; get; } = 1;
    }

    private class SizeBalancedTreeMap<TK> where TK : IComparable<TK>, IEquatable<TK>
    {
        private SbtNode<TK>? _root;

        private static SbtNode<TK> RightRotate(SbtNode<TK>? cur)
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

        private static SbtNode<TK> LeftRotate(SbtNode<TK>? cur)
        {
            var rightNode = cur?.RightChild;
            if (rightNode != null)
            {
                cur!.RightChild = rightNode.LeftChild;
                rightNode.LeftChild = cur;
                rightNode.Size = cur.Size;
                cur.Size = (cur.LeftChild?.Size ?? 0) + (cur.RightChild?.Size ?? 0) + 1;
                return rightNode;
            }

            throw new InvalidOperationException();
        }

        private SbtNode<TK>? Maintain(SbtNode<TK>? cur)
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
                cur.LeftChild = LeftRotate(cur.LeftChild ?? throw new InvalidOperationException());
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

        private SbtNode<TK>? FindLastIndex(TK key)
        {
            var pre = _root;
            var cur = _root;
            while (cur != null)
            {
                pre = cur;
                if (key.CompareTo(cur.Key) == 0)
                    break;
                cur = key.CompareTo(cur.Key) < 0 ? cur.LeftChild : cur.RightChild;
            }

            return pre;
        }

        private SbtNode<TK>? Add(SbtNode<TK>? cur, TK key)
        {
            if (cur == null) return new SbtNode<TK>(key);

            cur.Size++;
            if (key.CompareTo(cur.Key) < 0)
                cur.LeftChild = Add(cur.LeftChild, key);
            else
                cur.RightChild = Add(cur.RightChild, key);

            return Maintain(cur);
        }

        private static SbtNode<TK>? Delete(SbtNode<TK>? cur, TK key)
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
                        SbtNode<TK>? pre = null;
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

                return cur;
            }

            throw new InvalidOperationException();
        }

        private static SbtNode<TK> GetIndex(SbtNode<TK>? cur, int kth)
        {
            if (cur != null)
            {
                if (kth == (cur.LeftChild?.Size ?? 0) + 1)
                    return cur;
                if (kth <= (cur.LeftChild?.Size ?? 0))
                    return GetIndex(cur.LeftChild, kth);
                return GetIndex(cur.RightChild, kth - (cur.LeftChild?.Size ?? 0) - 1);
            }

            throw new InvalidOperationException();
        }

        public int Size()
        {
            return _root?.Size ?? 0;
        }

        private bool ContainsKey(TK key)
        {
            if (key.Equals(default)) throw new Exception("invalid parameter.");

            var lastNode = FindLastIndex(key);
            return lastNode != null && key.CompareTo(lastNode.Key) == 0;
        }

        public void Add(TK key)
        {
            if (key.Equals(default)) throw new Exception("invalid parameter.");

            var lastNode = FindLastIndex(key);
            if (lastNode == null || key.CompareTo(lastNode.Key) != 0) _root = Add(_root, key);
        }

        public void Remove(TK key)
        {
            if (key.Equals(default)) throw new Exception("invalid parameter.");

            if (ContainsKey(key)) _root = Delete(_root, key);
        }

        public TK GetIndexKey(int index)
        {
            if (index < 0 || index >= Size()) throw new Exception("invalid parameter.");

            return GetIndex(_root, index + 1).Key;
        }
    }

    private class Node(int i, int v) : IComparable<Node>, IEquatable<Node>
    {
        private readonly int _index = i;
        public readonly int Value = v;

        public int CompareTo(Node? o)
        {
            if (o == null) throw new InvalidOperationException();

            return Value != o.Value
                ? Convert.ToInt32(Value).CompareTo(o.Value)
                : Convert.ToInt32(_index).CompareTo(o._index);
        }

        public bool Equals(Node? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _index == other._index && Value == other.Value;
        }
    }
}

public class Test
{
    public static void Run()
    {
        int[] numbers = [1, 3, -1, -3, 5, 3, 6, 7];
        const int k = 3;

        foreach (var item in SlidingWindowMedian.MedianSlidingWindow(numbers, k)) Console.WriteLine(item);
    }
}