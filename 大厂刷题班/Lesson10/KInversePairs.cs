//pass

namespace AdvancedTraining.Lesson10;

// 测试链接 : https://leetcode.cn/problems/k-inverse-pairs-array/
public class KInversePairs
{
    private static int KInversePairs1(int n, int k)
    {
        if (n < 1 || k < 0) return 0;
        var dp = new int[n + 1, k + 1];
        dp[0, 0] = 1;
        var mod = 1000000007;
        for (var i = 1; i <= n; i++)
        {
            dp[i, 0] = 1;
            for (var j = 1; j <= k; j++)
            {
                dp[i, j] = (dp[i, j - 1] + dp[i - 1, j]) % mod;
                if (j >= i) dp[i, j] = (dp[i, j] - dp[i - 1, j - i] + mod) % mod;
            }
        }

        return dp[n, k];
    }

    private static int KInversePairs2(int n, int k)
    {
        if (n < 1 || k < 0) return 0;
        var dp = new int[n + 1, k + 1];
        dp[0, 0] = 1;
        for (var i = 1; i <= n; i++)
        {
            dp[i, 0] = 1;
            for (var j = 1; j <= k; j++)
            {
                dp[i, j] = dp[i - 1, j] + dp[i, j - 1];
                if (j >= i) dp[i, j] -= dp[i - 1, j - i];
            }
        }

        return dp[n, k];
    }


    public static void Run()
    {
        Console.WriteLine(KInversePairs1(3, 0)); //输出1

        Console.WriteLine(KInversePairs2(3, 0)); //输出1
    }
}