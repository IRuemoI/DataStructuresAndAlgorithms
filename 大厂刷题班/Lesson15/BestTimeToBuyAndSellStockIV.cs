//pass

namespace AdvancedTraining.Lesson15;

//leetcode 188
public class BestTimeToBuyAndSellStockIv
{
    private static int MaxProfit(int k, int[]? prices)
    {
        if (prices == null || prices.Length == 0) return 0;
        var n = prices.Length;
        if (k >= n / 2) return AllTrans(prices);
        var dp = new int[k + 1, n];
        var ans = 0;
        for (var tran = 1; tran <= k; tran++)
        {
            var pre = dp[tran, 0];
            var best = pre - prices[0];
            for (var index = 1; index < n; index++)
            {
                pre = dp[tran - 1, index];
                dp[tran, index] = Math.Max(dp[tran, index - 1], prices[index] + best);
                best = Math.Max(best, pre - prices[index]);
                ans = Math.Max(dp[tran, index], ans);
            }
        }

        return ans;
    }

    private static int AllTrans(int[] prices)
    {
        var ans = 0;
        for (var i = 1; i < prices.Length; i++) ans += Math.Max(prices[i] - prices[i - 1], 0);
        return ans;
    }

    // 课上写的版本，对了
    private static int MaxProfit2(int k, int[]? arr)
    {
        if (arr == null || arr.Length == 0 || k < 1) return 0;
        var n = arr.Length;
        if (k >= n / 2) return AllTrans(arr);
        var dp = new int[n, k + 1];
        // dp[...,0] = 0
        // dp[0,...] = arr[0.0] 0
        for (var j = 1; j <= k; j++)
        {
            // dp[1,j]
            var p1 = dp[0, j];
            var best = Math.Max(dp[1, j - 1] - arr[1], dp[0, j - 1] - arr[0]);
            dp[1, j] = Math.Max(p1, best + arr[1]);
            // dp[1,j] 准备好一些枚举，接下来准备好的枚举
            for (var i = 2; i < n; i++)
            {
                p1 = dp[i - 1, j];
                var newP = dp[i, j - 1] - arr[i];
                best = Math.Max(newP, best);
                dp[i, j] = Math.Max(p1, best + arr[i]);
            }
        }

        return dp[n - 1, k];
    }

    public static void Run()
    {
        Console.WriteLine(MaxProfit(2, [3, 2, 6, 5, 0, 3])); //输出7
        Console.WriteLine(MaxProfit2(2, [3, 2, 6, 5, 0, 3])); //输出7
    }
}