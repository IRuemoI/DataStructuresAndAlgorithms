#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson47;

// 只支持单点增加 + 范围查询的动态开点线段树（累加和）
// todo:待整理
public class DynamicSegmentTree02
{
    public static void Run()
    {
        const int size = 10000;
        const int testTime = 50000;
        const int value = 500;
        var dst = new DynamicSegmentTree(size);
        var right = new Right(size);
        Console.WriteLine("测试开始");
        for (var k = 0; k < testTime; k++)
            if (Utility.getRandomDouble < 0.5)
            {
                var i = (int)(Utility.getRandomDouble * size) + 1;
                var v = (int)(Utility.getRandomDouble * value);
                dst.Add(i, v);
                right.Add(i, v);
            }
            else
            {
                var a = (int)(Utility.getRandomDouble * size) + 1;
                var b = (int)(Utility.getRandomDouble * size) + 1;
                var s = Math.Min(a, b);
                var e = Math.Max(a, b);
                var ans1 = dst.Query(s, e);
                var ans2 = right.Query(s, e);
                if (ans1 != ans2)
                {
                    Console.WriteLine("出错了!");
                    Console.WriteLine(ans1);
                    Console.WriteLine(ans2);
                }
            }

        Console.WriteLine("测试结束");
    }

    private class Node
    {
        public Node? left;
        public Node? right;
        public int sum;
    }

    // arr[0] -> 1
    // 线段树，从1开始下标!
    private class DynamicSegmentTree(int max)
    {
        private readonly Node? _root = new();

        // 下标i这个位置的数，增加v
        public void Add(int i, int v)
        {
            Add(_root, 1, max, i, v);
        }

        // c-> cur 当前节点！表达的范围 l~r
        // i位置的数，增加v
        // 潜台词！i一定在l~r范围上！
        private void Add(Node c, int l, int r, int i, int v)
        {
            if (l == r)
            {
                c.sum += v;
            }
            else
            {
                // l~r 还可以划分
                var mid = (l + r) / 2;
                if (i <= mid)
                {
                    // l ~ mid
                    if (c.left == null) c.left = new Node();
                    Add(c.left, l, mid, i, v);
                }
                else
                {
                    // mid + 1 ~ r
                    if (c.right == null) c.right = new Node();
                    Add(c.right, mid + 1, r, i, v);
                }

                c.sum = (c.left?.sum ?? 0) + (c.right?.sum ?? 0);
            }
        }

        // s~e范围的累加和，告诉我！
        public int Query(int s, int e)
        {
            return Query(_root, 1, max, s, e);
        }

        // 当前节点c，表达的范围l~r
        // 收到了一个任务，s~e这个任务！
        // s~e这个任务，影响了多少l~r范围的数，把答案返回！
        private int Query(Node c, int l, int r, int s, int e)
        {
            if (c == null) return 0;
            if (s <= l && r <= e)
                // 3~6  1~100任务
                return c.sum;
            // 有影响，但又不是全影响
            // l ~ r
            // l~mid    mid+1~r
            var mid = (l + r) / 2;
            // 1~100  
            // 1~50  51 ~ 100 
            // 任务  s~e  53~76
            if (e <= mid)
                return Query(c.left, l, mid, s, e);
            if (s > mid)
                return Query(c.right, mid + 1, r, s, e);
            return Query(c.left, l, mid, s, e) + Query(c.right, mid + 1, r, s, e);
        }
    }

    private class Right
    {
        private readonly int[] arr;

        public Right(int size)
        {
            arr = new int[size + 1];
        }

        public void Add(int i, int v)
        {
            arr[i] += v;
        }

        public int Query(int s, int e)
        {
            var sum = 0;
            for (var i = s; i <= e; i++) sum += arr[i];
            return sum;
        }
    }
}