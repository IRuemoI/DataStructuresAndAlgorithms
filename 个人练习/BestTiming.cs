namespace CustomTraining;

public class BestTiming
{
    private static int BestTimingCode(int[] prices)
    {
        var cost = int.MaxValue;
        var profit = 0;
        foreach (var price in prices)
        {
            cost = Math.Min(cost, price);
            profit = Math.Max(profit, price - cost);
        }

        return profit;
    }

    public static void Run()
    {
        Console.WriteLine(BestTimingCode([3, 6, 2, 9, 8, 5]));
    }
}