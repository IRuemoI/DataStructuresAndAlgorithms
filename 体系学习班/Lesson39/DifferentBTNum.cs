//测试通过

namespace Algorithms.Lesson39;

public static class DifferentBtNum
{
    //	k(0) = 1, k(1) = 1
    //
    //	k(n) = k(0) * k(n - 1) + k(1) * k(n - 2) + ... + k(n - 2) * k(1) + k(n - 1) * k(0)
    //	或者
    //	k(n) = c(2n, n) / (n + 1)
    //	或者
    //	k(n) = c(2n, n) - c(2n, n-1)

    public static long Num1(int n)
    {
        if (n < 0) return 0;
        if (n < 2) return 1;
        var dp = new long[n + 1];
        dp[0] = 1;
        dp[1] = 1;
        for (var i = 2; i <= n; i++)
        for (var leftSize = 0; leftSize < i; leftSize++)
            dp[i] += dp[leftSize] * dp[i - 1 - leftSize];
        return dp[n];
    }

    public static long Num2(int n)
    {
        if (n < 0) return 0;
        if (n < 2) return 1;
        long a = 1;
        long b = 1;
        for (int i = 1, j = n + 1; i <= n; i++, j++)
        {
            a *= i;
            b *= j;
            var gcd = Gcd(a, b);
            a /= gcd;
            b /= gcd;
        }

        return b / a / (n + 1);
    }

    private static long Gcd(long m, long n)
    {
        return n == 0 ? m : Gcd(n, m % n);
    }
}

public static class DifferentBtNumTest
{
    public static void Run()
    {
        Console.WriteLine("测试开始");
        for (var i = 0; i < 15; i++)
        {
            var ans1 = DifferentBtNum.Num1(i);
            var ans2 = DifferentBtNum.Num2(i);
            if (ans1 != ans2) Console.WriteLine("出错啦！");
        }

        Console.WriteLine("测试完成");
    }
}