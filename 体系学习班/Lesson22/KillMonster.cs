//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson22;

public class KillMonster
{
    private static double Right(int n, int m, int k)
    {
        if (n < 1 || m < 1 || k < 1) return 0;

        var all = (long)Math.Pow(m + 1, k);
        var kill = Process(k, m, n);
        return (double)kill / all;
    }

    // 怪兽还剩hp点血
    // 每次的伤害在[0~M]范围上
    // 还有times次可以砍
    // 返回砍死的情况数！
    private static long Process(int times, int maxDamage, int hp)
    {
        if (times == 0) return hp <= 0 ? 1 : 0;

        if (hp <= 0) return (long)Math.Pow(maxDamage + 1, times);

        long ways = 0;
        for (var i = 0; i <= maxDamage; i++) ways += Process(times - 1, maxDamage, hp - i);

        return ways;
    }

    private static double Dp1(int n, int m, int k)
    {
        if (n < 1 || m < 1 || k < 1) return 0;

        var all = (long)Math.Pow(m + 1, k);
        var dp = new long[k + 1, n + 1];
        dp[0, 0] = 1;
        for (var times = 1; times <= k; times++)
        {
            dp[times, 0] = (long)Math.Pow(m + 1, times);
            for (var hp = 1; hp <= n; hp++)
            {
                long ways = 0;
                for (var i = 0; i <= m; i++)
                    if (hp - i >= 0)
                        ways += dp[times - 1, hp - i];
                    else
                        ways += (long)Math.Pow(m + 1, times - 1);

                dp[times, hp] = ways;
            }
        }

        var kill = dp[k, n];
        return kill / (double)all;
    }

    private static double Dp2(int n, int m, int k)
    {
        if (n < 1 || m < 1 || k < 1) return 0;

        var all = (long)Math.Pow(m + 1, k);
        var dp = new long[k + 1, n + 1];
        dp[0, 0] = 1;
        for (var times = 1; times <= k; times++)
        {
            dp[times, 0] = (long)Math.Pow(m + 1, times);
            for (var hp = 1; hp <= n; hp++)
            {
                dp[times, hp] = dp[times, hp - 1] + dp[times - 1, hp];
                if (hp - 1 - m >= 0)
                    dp[times, hp] -= dp[times - 1, hp - 1 - m];
                else
                    dp[times, hp] -= (long)Math.Pow(m + 1, times - 1);
            }
        }

        var kill = dp[k, n];
        return kill / (double)all;
    }

    public static void Run()
    {
        const int nMax = 10;
        const int mMax = 10;
        const int kMax = 10;
        var testTime = 200;
        Console.WriteLine("测试开始");

        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(Utility.getRandomDouble * nMax);
            var m = (int)(Utility.getRandomDouble * mMax);
            var k = (int)(Utility.getRandomDouble * kMax);
            var ans1 = Right(n, m, k);
            var ans2 = Dp1(n, m, k);
            var ans3 = Dp2(n, m, k);
            if (Math.Abs(ans1 - ans2) > 0.001f || Math.Abs(ans1 - ans3) > 0.001f)
            {
                Console.WriteLine("出错啦！");
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}