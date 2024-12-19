//pass
namespace AdvancedTraining.Lesson15;

// leetcode 121
public class BestTimeToBuyAndSellStock
{
    private static int MaxProfit(int[]? prices)
    {
        if (prices == null || prices.Length == 0) return 0;
        // 必须在0时刻卖掉，[0] - [0]
        var ans = 0;
        // arr[0...0]
        var min = prices[0];
        for (var i = 1; i < prices.Length; i++)
        {
            min = Math.Min(min, prices[i]);
            ans = Math.Max(ans, prices[i] - min);
        }

        return ans;
    }
}