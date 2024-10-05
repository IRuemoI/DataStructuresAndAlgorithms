#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson26;

public class MinRange
{
    private static int[] SmallestRange(IList<IList<int>> numbers)
    {
        var n = numbers.Count;
        var set = new SortedSet<Node>(new NodeComparator());
        for (var i = 0; i < n; i++) set.Add(new Node(numbers[i][0], i, 0));

        var r = int.MaxValue;
        var a = 0;
        var b = 0;
        while (set.Count == n)
        {
            var max = set.Max;
            var min = set.First();
            set.Remove(set.First());
            if (max != null && max.Val - min.Val < r)
            {
                r = max.Val - min.Val;
                a = min.Val;
                b = max.Val;
            }

            if (min.Idx < numbers[min.Arr].Count - 1)
                set.Add(new Node(numbers[min.Arr][min.Idx + 1], min.Arr, min.Idx + 1));
        }

        return new[] { a, b };
    }

    // 以下为课堂题目，在main函数里有对数器
    private static int MinRange1(int[][] m)
    {
        var min = int.MaxValue;
        for (var i = 0; i < m[0].Length; i++)
        for (var j = 0; j < m[1].Length; j++)
        for (var k = 0; k < m[2].Length; k++)
            min = Math.Min(min,
                Math.Abs(m[0][i] - m[1][j]) + Math.Abs(m[1][j] - m[2][k]) + Math.Abs(m[2][k] - m[0][i]));

        return min;
    }

    private static int MinRange2(int[][] matrix)
    {
        var n = matrix.Length;
        var set = new SortedSet<Node>(new NodeComparator());
        for (var i = 0; i < n; i++) set.Add(new Node(matrix[i][0], i, 0));

        var min = int.MaxValue;
        while (set.Count == n)
        {
            if (set is { Max: not null, Min: not null })
                min = Math.Min(min, set.Max.Val - set.Min.Val);

            var cur = set.First();
            set.Remove(set.First());
            if (cur.Idx < matrix[cur.Arr].Length - 1)
                set.Add(new Node(matrix[cur.Arr][cur.Idx + 1], cur.Arr, cur.Idx + 1));
        }

        return min << 1;
    }

    private static int[][] GenerateRandomMatrix(int n, int v)
    {
        var m = new int[3][];
        for (var i = 0; i < 3; i++)
        {
            var s = (int)(Utility.getRandomDouble * n) + 1;
            m[i] = new int[s];
            for (var j = 0; j < s; j++) m[i][j] = (int)(Utility.getRandomDouble * v);

            Array.Sort(m[i]);
        }

        return m;
    }

    public static void Run()
    {
        const int n = 20;
        const int v = 200;
        const int t = 1000000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < t; i++)
        {
            var m = GenerateRandomMatrix(n, v);
            var ans1 = MinRange1(m);
            var ans2 = MinRange2(m);
            if (ans1 != ans2) Console.WriteLine("出错了!");
        }

        Console.WriteLine("测试结束");
    }

    // 本题为求最小包含区间
    // 测试链接 :
    // https://leetcode.cn/problems/smallest-range-covering-elements-from-k-lists/
    private class Node(int value, int arrIndex, int index)
    {
        public readonly int Arr = arrIndex;
        public readonly int Idx = index;
        public readonly int Val = value;
    }

    private class NodeComparator : IComparer<Node>
    {
        public int Compare(Node? a, Node? b)
        {
            if (a is null || b is null) throw new Exception("a or b is null");
            return a.Val != b.Val ? a.Val - b.Val : a.Arr - b.Arr;
        }
    }
}