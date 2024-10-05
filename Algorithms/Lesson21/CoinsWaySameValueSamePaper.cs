//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson21;

public class CoinsWaySameValueSamePaper
{
    private static Info GetInfo(int[] arr)
    {
        var counts = new Dictionary<int, int>();
        foreach (var value in arr)
            if (!counts.ContainsKey(value))
                counts[value] = 1;
            else
                counts[value] += 1;

        var n = counts.Count;
        var coins = new int[n];
        var count = new int[n];
        var index = 0;
        foreach (var entry in counts)
        {
            coins[index] = entry.Key;
            count[index++] = entry.Value;
        }

        return new Info(coins, count);
    }

    private static int CoinsWay(int[]? arr, int aim)
    {
        if (arr == null || arr.Length == 0 || aim < 0) return 0;

        var info = GetInfo(arr);
        return Process(info.Coins, info.Count, 0, aim);
    }

    // coins 面值数组，正数且去重
    // count 每种面值对应的张数
    private static int Process(int[] coins, int[] count, int index, int rest)
    {
        if (index == coins.Length) return rest == 0 ? 1 : 0;

        var ways = 0;
        for (var zhang = 0; zhang * coins[index] <= rest && zhang <= count[index]; zhang++)
            ways += Process(coins, count, index + 1, rest - zhang * coins[index]);

        return ways;
    }

    private static int Dp1(int[]? arr, int aim)
    {
        if (arr == null || arr.Length == 0 || aim < 0) return 0;

        var info = GetInfo(arr);
        var coins = info.Coins;
        var count = info.Count;
        var n = coins.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 1;
        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
        {
            var ways = 0;
            for (var zhang = 0; zhang * coins[index] <= rest && zhang <= count[index]; zhang++)
                ways += dp[index + 1, rest - zhang * coins[index]];

            dp[index, rest] = ways;
        }

        return dp[0, aim];
    }

    private static int Dp2(int[]? arr, int aim)
    {
        if (arr == null || arr.Length == 0 || aim < 0) return 0;

        var info = GetInfo(arr);
        var coins = info.Coins;
        var count = info.Count;
        var n = coins.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 1;
        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
        {
            dp[index, rest] = dp[index + 1, rest];
            if (rest - coins[index] >= 0) dp[index, rest] += dp[index, rest - coins[index]];

            if (rest - coins[index] * (count[index] + 1) >= 0)
                dp[index, rest] -= dp[index + 1, rest - coins[index] * (count[index] + 1)];
        }

        return dp[0, aim];
    }

    // 为了测试
    private static int[] RandomArray(int maxLen, int maxValue)
    {
        var n = (int)(Utility.getRandomDouble * maxLen);
        var arr = new int[n];
        for (var i = 0; i < n; i++) arr[i] = (int)(Utility.getRandomDouble * maxValue) + 1;

        return arr;
    }

    // 为了测试
    private static void PrintArray(int[] arr)
    {
        foreach (var element in arr) Console.Write(element + " ");

        Console.WriteLine();
    }

    // 为了测试
    public static void Run()
    {
        var maxLen = 10;
        var maxValue = 20;
        var testTime = 1000000;
        Console.WriteLine("测试开始");

        for (var i = 0; i < testTime; i++)
        {
            var arr = RandomArray(maxLen, maxValue);
            var aim = (int)(Utility.getRandomDouble * maxValue);
            var ans1 = CoinsWay(arr, aim);
            var ans2 = Dp1(arr, aim);
            var ans3 = Dp2(arr, aim);
            if (ans1 != ans2 || ans1 != ans3)
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr);
                Console.WriteLine(aim);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine(ans3);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }

    private class Info
    {
        public readonly int[] Coins;
        public readonly int[] Count;

        public Info(int[] c, int[] z)
        {
            Coins = c;
            Count = z;
        }
    }
}