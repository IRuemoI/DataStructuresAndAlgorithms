//测试通过

#region

using Common.DataStructures.Heap;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson07;

public class CoverMax
{
    //暴力解
    private static int MaxCover1(int[,] lines)
    {
        var min = int.MaxValue;
        var max = int.MinValue;
        //先找出这组输入中左端最小值和右端最大值
        for (var i = 0; i < lines.GetLength(0); i++)
        {
            min = Math.Min(min, lines[i, 0]);
            max = Math.Max(max, lines[i, 1]);
        }

        var cover = 0;
        //对于每个"数轴上的点"，遍历所有线段获得在该点上的重叠数，并记录最大值
        for (var p = min + 0.5; p < max; p += 1)
        {
            var cur = 0;
            for (var i = 0; i < lines.GetLength(0); i++)
                if (lines[i, 0] < p && lines[i, 1] > p)
                    cur++;

            cover = Math.Max(cover, cur);
        }

        return cover;
    }

    private static int MaxCover2(int[,] m)
    {
        int maxCover = -1;
        var lines = new List<Line>();
        for (int i = 0; i < m.GetLength(0); i++) lines.Add(new Line(m[i, 0], m[i, 1]));
        lines.Sort();
        var minHeap = new Heap<int>((x, y) => x.CompareTo(y));
        foreach (var tempLine in lines)
        {
            //弹出堆中所有不大于将要添加线段的开始位置的所有值
            while (!minHeap.IsEmpty && minHeap.Peek()<= tempLine.LeftEnd)
            {
                minHeap.Pop();
            }

            //将当前线段的结束位置加入堆中
            minHeap.Push(tempLine.RightEnd);
            //记录最大覆盖数
            maxCover = Math.Max(maxCover, minHeap.Count);
        }
        return maxCover;
    }

    //用于测试
    private static int[,] GenerateLines(int n, int l, int r)
    {
        var size = (int)(Utility.GetRandomDouble * n) + 1;
        var ans = new int[size, 2];
        for (var i = 0; i < size; i++)
        {
            var a = l + (int)(Utility.GetRandomDouble * (r - l + 1));
            var b = l + (int)(Utility.GetRandomDouble * (r - l + 1));
            if (a == b) b = a + 1;

            ans[i, 0] = Math.Min(a, b);
            ans[i, 1] = Math.Max(a, b);
        }

        return ans;
    }

    public static void Run()
    {
        const int n = 100;
        const int l = 0;
        const int r = 200;
        const int testTimes = 10000;

        Console.WriteLine("测试开始");
        Utility.RestartStopwatch();
        for (var i = 0; i < testTimes; i++)
        {
            var lines = GenerateLines(n, l, r);
            var ans1 = MaxCover1(lines);
            var ans2 = MaxCover2(lines);
            if (ans1 != ans2) Console.WriteLine("出错啦！");
        }

        Console.WriteLine($"测试结束，总耗时:{Utility.GetStopwatchElapsedMilliseconds()}ms");
    }

    private class Line(int leftEnd, int rightEnd) : IComparable<Line>
    {
        public readonly int LeftEnd = leftEnd;
        public readonly int RightEnd = rightEnd;

        public int CompareTo(Line? other)
        {
            if (other != null)
                return LeftEnd - other.LeftEnd;
            throw new ArgumentNullException();
        }
    }
}