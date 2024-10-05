//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson41;

// 四边形不等式：合并石子问题
public class StoneMerge
{
    private static int[] Sum(int[] arr)
    {
        var n = arr.Length;
        var s = new int[n + 1];
        s[0] = 0;
        for (var i = 0; i < n; i++) s[i + 1] = s[i] + arr[i];

        return s;
    }

    private static int W(int[] s, int l, int r)
    {
        return s[r + 1] - s[l];
    }

    private static int Min1(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        var n = arr.Length;
        var s = Sum(arr);
        return Process1(0, n - 1, s);
    }

    private static int Process1(int l, int r, int[] s)
    {
        if (l == r) return 0;

        var next = int.MaxValue;
        for (var leftEnd = l; leftEnd < r; leftEnd++)
            next = Math.Min(next, Process1(l, leftEnd, s) + Process1(leftEnd + 1, r, s));

        return next + W(s, l, r);
    }

    private static int Min2(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        var n = arr.Length;
        var s = Sum(arr);
        var dp = new int[n, n];
        // dp[i,i] = 0
        for (var l = n - 2; l >= 0; l--)
        for (var r = l + 1; r < n; r++)
        {
            var next = int.MaxValue;
            // dp(L..leftEnd)  + dp[leftEnd+1...R]  + 累加和[L...R]
            for (var leftEnd = l; leftEnd < r; leftEnd++) next = Math.Min(next, dp[l, leftEnd] + dp[leftEnd + 1, r]);

            dp[l, r] = next + W(s, l, r);
        }

        return dp[0, n - 1];
    }

    private static int Min3(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        var n = arr.Length;
        var s = Sum(arr);
        var dp = new int[n, n];
        var best = new int[n, n];
        for (var i = 0; i < n - 1; i++)
        {
            best[i, i + 1] = i;
            dp[i, i + 1] = W(s, i, i + 1);
        }

        for (var l = n - 3; l >= 0; l--)
        for (var r = l + 2; r < n; r++)
        {
            var next = int.MaxValue;
            var choose = -1;
            for (var leftEnd = best[l, r - 1]; leftEnd <= best[l + 1, r]; leftEnd++)
            {
                var cur = dp[l, leftEnd] + dp[leftEnd + 1, r];
                if (cur <= next)
                {
                    next = cur;
                    choose = leftEnd;
                }
            }

            best[l, r] = choose;
            dp[l, r] = next + W(s, l, r);
        }

        return dp[0, n - 1];
    }

    private static int[] RandomArray(int len, int maxValue)
    {
        var arr = new int[len];
        for (var i = 0; i < len; i++) arr[i] = (int)(Utility.getRandomDouble * maxValue);

        return arr;
    }

    public static void Run()
    {
        var n = 15;
        var maxValue = 100;
        var testTime = 1000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(Utility.getRandomDouble * n);
            var arr = RandomArray(len, maxValue);
            var ans1 = Min1(arr);
            var ans2 = Min2(arr);
            var ans3 = Min3(arr);
            if (ans1 != ans2 || ans1 != ans3) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试结束");
    }
}