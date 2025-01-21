namespace AdvancedTraining.Lesson47;

// 利用只支持单点增加 + 范围查询的动态开点线段树（累加和），解决leetcode 315
//https://leetcode.cn/problems/count-of-smaller-numbers-after-self/description/
public class CountOfSmallerNumbersAfterSelf //leetcode_0315
{
    private static List<int> CountSmaller(int[]? numbers)
    {
        var ans = new List<int>();
        if (numbers == null || numbers.Length == 0) return ans;
        var n = numbers.Length;
        for (var i = 0; i < n; i++) ans.Add(0);
        var arr = new int[n][];
        for (var i = 0; i < n; i++) arr[i] = new[] { numbers[i], i };
        Array.Sort(arr, (a, b) => a[0] - b[0]);
        var dst = new DynamicSegmentTree(n);
        foreach (var num in arr)
        {
            ans[num[1]] = dst.Query(num[1] + 1, n);
            dst.Add(num[1] + 1, 1);
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(string.Join(",", CountSmaller([5, 2, 6, 1]))); //输出：[2,1,1,0] 
    }

    private class Node
    {
        public Node? Left;
        public Node? Right;
        public int Sum;
    }

    private class DynamicSegmentTree(int max)
    {
        private readonly Node _root = new();

        public void Add(int i, int v)
        {
            Add(_root, 1, max, i, v);
        }

        private void Add(Node c, int l, int r, int i, int v)
        {
            if (l == r)
            {
                c.Sum += v;
            }
            else
            {
                var mid = (l + r) / 2;
                if (i <= mid)
                {
                    c.Left ??= new Node();
                    Add(c.Left, l, mid, i, v);
                }
                else
                {
                    c.Right ??= new Node();
                    Add(c.Right, mid + 1, r, i, v);
                }

                c.Sum = (c.Left?.Sum ?? 0) + (c.Right?.Sum ?? 0);
            }
        }

        public int Query(int s, int e)
        {
            return Query(_root, 1, max, s, e);
        }

        private int Query(Node? c, int l, int r, int s, int e)
        {
            if (c == null) return 0;
            if (s <= l && r <= e) return c.Sum;
            var mid = (l + r) / 2;
            if (e <= mid)
                return Query(c.Left, l, mid, s, e);
            if (s > mid)
                return Query(c.Right, mid + 1, r, s, e);
            return Query(c.Left, l, mid, s, e) + Query(c.Right, mid + 1, r, s, e);
        }
    }
}