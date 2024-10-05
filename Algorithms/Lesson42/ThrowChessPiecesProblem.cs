//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson42;

// leetcode测试链接：https://leetcode.cn/problems/super-egg-drop
// 方法1和方法2会超时
// 方法3勉强通过
// 方法4打败100%
// 方法5打败100%，方法5是在方法4的基础上做了进一步的常数优化
public class ThrowChessPiecesProblem
{
    // private static int SuperEggDrop1(int kChess, int nLevel)
    // {
    //     if (nLevel < 1 || kChess < 1) return 0;
    //     return Process1(nLevel, kChess);
    // }

    // rest还剩多少层楼需要去验证
    // k还有多少颗棋子能够使用
    // 一定要验证出最高的不会碎的楼层！但是每次都是坏运气。
    // 返回至少需要扔几次？
    // private static int Process1(int rest, int k)
    // {
    //     if (rest == 0) return 0;
    //     if (k == 1) return rest;
    //     var min = int.MaxValue;
    //     for (var i = 1; i != rest + 1; i++)
    //         // 第一次扔的时候，仍在了i层
    //         min = Math.Min(min, Math.Max(Process1(i - 1, k - 1), Process1(rest - i, k)));
    //     return min + 1;
    // }

    private static int SuperEggDrop2(int kChess, int nLevel)
    {
        if (nLevel < 1 || kChess < 1) return 0;
        if (kChess == 1) return nLevel;
        var dp = new int[nLevel + 1, kChess + 1];
        for (var i = 1; i != dp.Length; i++) dp[i, 1] = i;
        for (var i = 1; i != dp.Length; i++)
        for (var j = 2; j != dp.GetLength(1); j++)
        {
            var min = int.MaxValue;
            for (var k = 1; k != i + 1; k++) min = Math.Min(min, Math.Max(dp[k - 1, j - 1], dp[i - k, j]));
            dp[i, j] = min + 1;
        }

        return dp[nLevel, kChess];
    }

    private static int SuperEggDrop3(int kChess, int nLevel)
    {
        if (nLevel < 1 || kChess < 1) return 0;
        if (kChess == 1) return nLevel;
        var dp = new int[nLevel + 1, kChess + 1];
        for (var i = 1; i != dp.GetLength(0); i++) dp[i, 1] = i;
        var best = new int[nLevel + 1, kChess + 1];
        for (var i = 1; i != dp.GetLength(1); i++)
        {
            dp[1, i] = 1;
            best[1, i] = 1;
        }

        for (var i = 2; i < nLevel + 1; i++)
        for (var j = kChess; j > 1; j--)
        {
            var ans = int.MaxValue;
            var bestChoose = -1;
            var down = best[i - 1, j];
            var up = j == kChess ? i : best[i, j + 1];
            for (var first = down; first <= up; first++)
            {
                var cur = Math.Max(dp[first - 1, j - 1], dp[i - first, j]);
                if (cur <= ans)
                {
                    ans = cur;
                    bestChoose = first;
                }
            }

            dp[i, j] = ans + 1;
            best[i, j] = bestChoose;
        }

        return dp[nLevel, kChess];
    }

    private static int SuperEggDrop4(int kChess, int nLevel)
    {
        if (nLevel < 1 || kChess < 1) return 0;
        var dp = new int[kChess];
        var res = 0;
        while (true)
        {
            res++;
            var previous = 0;
            for (var i = 0; i < dp.Length; i++)
            {
                var tmp = dp[i];
                dp[i] = dp[i] + previous + 1;
                previous = tmp;
                if (dp[i] >= nLevel) return res;
            }
        }
    }

    private static int SuperEggDrop5(int kChess, int nLevel)
    {
        if (nLevel < 1 || kChess < 1) return 0;
        var bsTimes = Log2N(nLevel) + 1;
        if (kChess >= bsTimes) return bsTimes;
        var dp = new int[kChess];
        var res = 0;
        while (true)
        {
            res++;
            var previous = 0;
            for (var i = 0; i < dp.Length; i++)
            {
                var tmp = dp[i];
                dp[i] = dp[i] + previous + 1;
                previous = tmp;
                if (dp[i] >= nLevel) return res;
            }
        }
    }

    private static int Log2N(int n)
    {
        var res = -1;
        while (n != 0)
        {
            res++;
            n >>>= 1;
        }

        return res;
    }

    public static void Run()
    {
        const int maxN = 500;
        const int maxK = 30;
        const int testTime = 1;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(Utility.getRandomDouble * maxN) + 1;
            var k = (int)(Utility.getRandomDouble * maxK) + 1;
            var ans2 = SuperEggDrop2(k, n);
            var ans3 = SuperEggDrop3(k, n);
            var ans4 = SuperEggDrop4(k, n);
            var ans5 = SuperEggDrop5(k, n);
            if (ans2 != ans3 || ans4 != ans5 || ans2 != ans4) Console.WriteLine("出错了!");
        }

        Console.WriteLine("测试结束");
    }
}