//pass

namespace AdvancedTraining.Lesson15;

//leetcode 122
public class BestTimeToBuyAndSellStockIi
{
    private static int MaxProfit(int[]? prices)
    {
        if (prices == null || prices.Length == 0) return 0;
        var ans = 0;
        for (var i = 1; i < prices.Length; i++) ans += Math.Max(prices[i] - prices[i - 1], 0);
        return ans;
    }
}