//pass

namespace AdvancedTraining.Lesson15;

//leetcode 123
public class BestTimeToBuyAndSellStockIii
{
    private static int MaxProfit(int[]? prices)
    {
        if (prices == null || prices.Length < 2) return 0;
        var ans = 0;
        var doneOnceMinusBuyMax = -prices[0];
        var doneOnceMax = 0;
        var min = prices[0];
        for (var i = 1; i < prices.Length; i++)
        {
            min = Math.Min(min, prices[i]);
            ans = Math.Max(ans, doneOnceMinusBuyMax + prices[i]);
            doneOnceMax = Math.Max(doneOnceMax, prices[i] - min);
            doneOnceMinusBuyMax = Math.Max(doneOnceMinusBuyMax, doneOnceMax - prices[i]);
        }

        return ans;
    }
}