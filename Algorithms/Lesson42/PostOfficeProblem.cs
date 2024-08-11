//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson42;

public class PostOfficeProblem
{
    private static int Min1(int[]? arr, int num)
    {
        if (arr == null || num < 1 || arr.Length < num) return 0;

        var n = arr.Length;
        var w = new int[n + 1, n + 1];
        for (var l = 0; l < n; l++)
        for (var r = l + 1; r < n; r++)
            w[l, r] = w[l, r - 1] + arr[r] - arr[(l + r) >> 1];
        var dp = new int[n, num + 1];
        for (var i = 0; i < n; i++) dp[i, 1] = w[0, i];

        for (var i = 1; i < n; i++)
        for (var j = 2; j <= Math.Min(i, num); j++)
        {
            var ans = int.MaxValue;
            for (var k = 0; k <= i; k++) ans = Math.Min(ans, dp[k, j - 1] + w[k + 1, i]);

            dp[i, j] = ans;
        }

        return dp[n - 1, num];
    }

    private static int Min2(int[]? arr, int num)
    {
        if (arr == null || num < 1 || arr.Length < num) return 0;

        var n = arr.Length;
        var w = new int[n + 1, n + 1];
        for (var l = 0; l < n; l++)
        for (var r = l + 1; r < n; r++)
            w[l, r] = w[l, r - 1] + arr[r] - arr[(l + r) >> 1];
        var dp = new int[n, num + 1];
        var best = new int[n, num + 1];
        for (var i = 0; i < n; i++)
        {
            dp[i, 1] = w[0, i];
            best[i, 1] = -1;
        }

        for (var j = 2; j <= num; j++)
        for (var i = n - 1; i >= j; i--)
        {
            var down = best[i, j - 1];
            var up = i == n - 1 ? n - 1 : best[i + 1, j];
            var ans = int.MaxValue;
            var bestChoose = -1;
            for (var leftEnd = down; leftEnd <= up; leftEnd++)
            {
                var leftCost = leftEnd == -1 ? 0 : dp[leftEnd, j - 1];
                var rightCost = leftEnd == i ? 0 : w[leftEnd + 1, i];
                var cur = leftCost + rightCost;
                if (cur <= ans)
                {
                    ans = cur;
                    bestChoose = leftEnd;
                }
            }

            dp[i, j] = ans;
            best[i, j] = bestChoose;
        }

        return dp[n - 1, num];
    }

    //用于测试
    private static int[] RandomSortedArray(int len, int range)
    {
        var arr = new int[len];
        for (var i = 0; i != len; i++) arr[i] = (int)(Utility.GetRandomDouble * range);

        Array.Sort(arr);
        return arr;
    }

    //用于测试
    private static void PrintArray(int[] arr)
    {
        for (var i = 0; i != arr.Length; i++) Console.Write(arr[i] + " ");

        Console.WriteLine();
    }

    //用于测试
    public static void Run()
    {
        var n = 30;
        var maxValue = 100;
        var testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.GetRandomDouble * n) + 1;
            var arr = RandomSortedArray(len, maxValue);
            var num = (int)(Utility.GetRandomDouble * n) + 1;
            var ans1 = Min1(arr, num);
            var ans2 = Min2(arr, num);
            if (ans1 != ans2)
            {
                PrintArray(arr);
                Console.WriteLine(num);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine("出错啦！");
            }
        }

        Console.WriteLine("测试结束");
    }
}