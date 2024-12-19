//测试通过

namespace Algorithms.Lesson22;

public class SplitNumber
{
    // n为正数
    private static int Ways(int n)
    {
        if (n < 0) return 0;
        if (n == 1) return 1;
        return Process(1, n);
    }

    // 上一个拆出来的数是pre
    // 还剩rest需要去拆
    // 返回拆解的方法数
    private static int Process(int pre, int rest)
    {
        if (rest == 0) return 1;
        if (pre > rest) return 0;
        var ways = 0;
        for (var first = pre; first <= rest; first++) ways += Process(first, rest - first);
        return ways;
    }

    private static int Dp1(int n)
    {
        if (n < 0) return 0;
        if (n == 1) return 1;
        var dp = new int[n + 1, n + 1];
        for (var pre = 1; pre <= n; pre++)
        {
            dp[pre, 0] = 1;
            dp[pre, pre] = 1;
        }

        for (var pre = n - 1; pre >= 1; pre--)
        for (var rest = pre + 1; rest <= n; rest++)
        {
            var ways = 0;
            for (var first = pre; first <= rest; first++) ways += dp[first, rest - first];
            dp[pre, rest] = ways;
        }

        return dp[1, n];
    }

    private static int Dp2(int n)
    {
        if (n < 0) return 0;
        if (n == 1) return 1;
        var dp = new int[n + 1, n + 1];
        for (var pre = 1; pre <= n; pre++)
        {
            dp[pre, 0] = 1;
            dp[pre, pre] = 1;
        }

        for (var pre = n - 1; pre >= 1; pre--)
        for (var rest = pre + 1; rest <= n; rest++)
        {
            dp[pre, rest] = dp[pre + 1, rest];
            dp[pre, rest] += dp[pre, rest - pre];
        }

        return dp[1, n];
    }

    public static void Run()
    {
        var test = 39;
        Console.WriteLine(Ways(test));
        Console.WriteLine(Dp1(test));
        Console.WriteLine(Dp2(test));
    }
}