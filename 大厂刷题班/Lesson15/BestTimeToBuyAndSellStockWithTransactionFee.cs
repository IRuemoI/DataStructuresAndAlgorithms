//pass

namespace AdvancedTraining.Lesson15;

//leetcode 714
public class BestTimeToBuyAndSellStockWithTransactionFee
{
    private static int MaxProfit(int[]? arr, int fee)
    {
        if (arr == null || arr.Length < 2) return 0;
        var n = arr.Length;
        // 0..0   0 -[0] - fee
        var bestBuy = -arr[0] - fee;
        // 0..0  卖  0
        var bestSell = 0;
        for (var i = 1; i < n; i++)
        {
            // 来到i位置了！
            // 如果在i必须买  收入 - 批发价 - fee
            var curBuy = bestSell - arr[i] - fee;
            // 如果在i必须卖  整体最优（收入 - 良好批发价 - fee）
            var curSell = bestBuy + arr[i];
            bestBuy = Math.Max(bestBuy, curBuy);
            bestSell = Math.Max(bestSell, curSell);
        }

        return bestSell;
    }


    public static void Run()
    {
        Console.WriteLine(MaxProfit([1, 3, 2, 8, 4, 9], 2)); //输出8
    }
}