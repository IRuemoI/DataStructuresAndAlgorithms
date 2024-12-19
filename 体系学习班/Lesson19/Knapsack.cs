//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson19;

public class Knapsack
{
    // 所有的货，重量和价值，都在w和v数组里
    // 为了方便，其中没有负数
    // bag背包容量，不能超过这个载重
    // 返回：不超重的情况下，能够得到的最大价值
    private static int MaxValue(int[]? w, int[]? v, int bag)
    {
        if (w == null || v == null || w.Length != v.Length || w.Length == 0) return 0;

        // 尝试函数！
        return Process(w, v, 0, bag);
    }

    // index 0~N
    // rest 负~bag
    private static int Process(int[] w, int[] v, int index, int rest)
    {
        if (rest < 0) return -1;

        if (index == w.Length) return 0;

        var p1 = Process(w, v, index + 1, rest);
        var p2 = 0;
        var next = Process(w, v, index + 1, rest - w[index]);
        if (next != -1) p2 = v[index] + next;

        return Math.Max(p1, p2);
    }

    private static int Dp(int[]? w, int[]? v, int bag)
    {
        if (w == null || v == null || w.Length != v.Length || w.Length == 0) return 0;

        var n = w.Length;
        var dp = new int[n + 1, bag + 1];
        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= bag; rest++)
        {
            var p1 = dp[index + 1, rest];
            var p2 = 0;
            var next = rest - w[index] < 0 ? -1 : dp[index + 1, rest - w[index]];
            if (next != -1) p2 = v[index] + next;

            dp[index, rest] = Math.Max(p1, p2);
        }

        return dp[0, bag];
    }

    public static void Run()
    {
        int[] weights = [3, 2, 4, 7, 3, 1, 7];
        int[] values = [5, 6, 3, 19, 12, 4, 2];
        const int bag = 15;

        Utility.RestartStopwatch();
        Console.WriteLine("方法1结果:" + MaxValue(weights, values, bag) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法2结果:" + Dp(weights, values, bag) + ",耗时:" + Utility.GetStopwatchElapsedMilliseconds() +
                          "ms");
    }
}