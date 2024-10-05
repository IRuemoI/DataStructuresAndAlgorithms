//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson37;

public class CountOfRangeSum
{
    private static int CountRangeSum1(int[] nums, int lower, int upper)
    {
        var n = nums.Length;
        var sums = new long[n + 1];
        for (var i = 0; i < n; ++i) sums[i + 1] = sums[i] + nums[i];

        return CountWhileMergeSort(sums, 0, n + 1, lower, upper);
    }

    private static int CountWhileMergeSort(long[] sums, int start, int end, int lower, int upper)
    {
        if (end - start <= 1) return 0;

        var mid = (start + end) / 2;
        var count = CountWhileMergeSort(sums, start, mid, lower, upper) +
                    CountWhileMergeSort(sums, mid, end, lower, upper);
        int j = mid, k = mid, t = mid;
        var cache = new long[end - start];
        for (int i = start, r = 0; i < mid; ++i, ++r)
        {
            while (k < end && sums[k] - sums[i] < lower) k++;

            while (j < end && sums[j] - sums[i] <= upper) j++;

            while (t < end && sums[t] < sums[i]) cache[r++] = sums[t++];

            cache[r] = sums[i];
            count += j - k;
        }

        Array.Copy(cache, 0, sums, start, t - start);
        return count;
    }

    private static int CountRangeSum2(int[] nums, int lower, int upper)
    {
        var treeSet = new SizeBalancedTreeSet();
        long sum = 0;
        var ans = 0;
        treeSet.Add(0); // 一个数都没有的时候，就已经有一个前缀和累加和为0，
        foreach (var item in nums)
        {
            sum += item;
            // sum    i结尾的时候[lower, upper]
            // 之前所有前缀累加和中，有多少累加和落在[sum - upper, sum - lower]
            // 查 ？ < sum - lower + 1   a
            // 查 ?  < sum - upper    b
            // a - b

            var a = treeSet.LessKeySize(sum - lower + 1);
            var b = treeSet.LessKeySize(sum - upper);
            ans += (int)(a - b);
            treeSet.Add(sum);
        }

        return ans;
    }

    //用于测试
    private static void PrintArray(int[] arr)
    {
        foreach (var item in arr)
            Console.Write(item + " ");

        Console.WriteLine();
    }

    //用于测试
    private static int[] GenerateArray(int len, int variable)
    {
        var arr = new int[len];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)(Utility.getRandomDouble * variable);

        return arr;
    }

    public static void Run()
    {
        var len = 200;
        var variable = 50;
        Console.WriteLine("test start");
        for (var i = 0; i < 10000; i++)
        {
            var test = GenerateArray(len, variable);
            var lower = (int)(Utility.getRandomDouble * variable) - (int)(Utility.getRandomDouble * variable);
            var upper = lower + (int)(Utility.getRandomDouble * variable);
            var ans1 = CountRangeSum1(test, lower, upper);
            var ans2 = CountRangeSum2(test, lower, upper);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错了");
                PrintArray(test);
                Console.WriteLine(lower);
                Console.WriteLine(upper);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
            }
        }

        Console.WriteLine("测试完成");
    }

    public class SbtNode
    {
        public readonly long Key;
        public long All; // 总的size
        public SbtNode? L;
        public SbtNode? R;
        public long Size; // 不同key的size

        public SbtNode(long k)
        {
            Key = k;
            Size = 1;
            All = 1;
        }
    }

    private class SizeBalancedTreeSet
    {
        private readonly HashSet<long> _set = new();
        private SbtNode? _root;

        protected virtual SbtNode RightRotate(SbtNode cur)
        {
            if (cur is not { L: not null, R: not null }) throw new InvalidOperationException();
            var same = cur.All - cur.L.All - cur.R.All;
            var leftNode = cur.L;
            cur.L = leftNode.R;
            leftNode.R = cur;
            leftNode.Size = cur.Size;
            if (cur.L != null)
            {
                cur.Size = cur.L.Size + cur.R.Size + 1;
                // all modify
                leftNode.All = cur.All;
                cur.All = cur.L.All + cur.R.All + same;
            }

            return leftNode;
        }

        protected virtual SbtNode LeftRotate(SbtNode cur)
        {
            if (cur is not { L: not null, R: not null }) throw new InvalidOperationException();
            var same = cur.All - cur.L.All - cur.R.All;
            var rightNode = cur.R;
            cur.R = rightNode.L;
            rightNode.L = cur;
            rightNode.Size = cur.Size;
            if (cur.R != null)
            {
                cur.Size = cur.L.Size + cur.R.Size + 1;
                // all modify
                rightNode.All = cur.All;
                cur.All = cur.L.All + cur.R.All + same;
            }

            return rightNode;
        }

        protected virtual SbtNode? Maintain(SbtNode? cur)
        {
            if (cur == null) return null;

            if (cur.L != null)
            {
                var leftSize = cur.L.Size;
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
                    cur.L = LeftRotate(cur.L);
                    cur = RightRotate(cur);
                    cur.L = Maintain(cur.L);
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
            }

            return cur;
        }

        protected virtual SbtNode? Add(SbtNode? cur, long key, bool contains)
        {
            if (cur == null) return new SbtNode(key);

            cur.All++;
            if (key == cur.Key) return cur;

            // 还在左滑或者右滑
            if (!contains) cur.Size++;

            if (key < cur.Key)
                cur.L = Add(cur.L, key, contains);
            else
                cur.R = Add(cur.R, key, contains);

            return Maintain(cur);
        }

        public virtual void Add(long sum)
        {
            var contains = _set.Contains(sum);
            _root = Add(_root, sum, contains);
            _set.Add(sum);
        }

        public virtual long LessKeySize(long key)
        {
            var cur = _root;
            long ans = 0;
            while (cur != null)
                if (key == cur.Key)
                {
                    return ans + (cur.L?.All ?? 0);
                }
                else if (key < cur.Key)
                {
                    cur = cur.L;
                }
                else
                {
                    ans += cur.All - (cur.R?.All ?? 0);
                    cur = cur.R;
                }

            return ans;
        }

        // > 7 8...
        // <8 ...<=7
        public virtual long MoreKeySize(long key)
        {
            return _root != null ? _root.All - LessKeySize(key + 1) : 0;
        }
    }
}