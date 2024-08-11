//测试通过

#region

using Common.DataStructures.Heap;
using Common.Utilities;

#endregion

namespace Algorithms.Lesson07;

public class CoverMax
{
    private static int MaxCover1(int[,] lines)
    {
        var min = int.MaxValue;
        var max = int.MinValue;
        for (var i = 0; i < lines.GetLength(0); i++)
        {
            min = Math.Min(min, lines[i, 0]);
            max = Math.Max(max, lines[i, 1]);
        }

        var cover = 0;
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
        var lines = new Line[m.GetLength(0)];
        for (var i = 0; i < m.GetLength(0); i++) lines[i] = new Line(m[i, 0], m[i, 1]);

        Array.Sort(lines);
        // 小根堆，每一条线段的结尾数值，使用默认的
        var minHeap = new Heap<int>((x, y) => x.CompareTo(y));
        var max = 0;
        foreach (var line in lines)
        {
            // lines[i] -> cur  在黑盒中，把<=cur.start 东西都弹出
            while (minHeap.Count != 0 && minHeap.Peek() <= line.Start) minHeap.Pop();

            minHeap.Push(line.End);
            max = Math.Max(max, minHeap.Count);
        }

        return max;
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
        const int testTimes = 200;

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

    private class Line : IComparable
    {
        public readonly int End;
        public readonly int Start;

        public Line(int s, int e)
        {
            Start = s;
            End = e;
        }

        public int CompareTo(object? obj)
        {
            if (obj is Line other)
                return Start - other.Start;
            throw new AggregateException();
        }
    }
}