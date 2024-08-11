//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson21;

public class CoinsWayEveryPaperDifferent
{
    private static int CoinWays(int[] arr, int aim)
    {
        return Process(arr, 0, aim);
    }

    // arr[index....] 组成正好rest这么多的钱，有几种方法
    private static int Process(int[] arr, int index, int rest)
    {
        if (rest < 0) return 0;
        if (index == arr.Length) // 没钱了！
            return rest == 0 ? 1 : 0;
        return Process(arr, index + 1, rest) + Process(arr, index + 1, rest - arr[index]);
    }

    private static int Dp(int[] arr, int aim)
    {
        if (aim == 0) return 1;
        var n = arr.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 1;
        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
            dp[index, rest] = dp[index + 1, rest] + (rest - arr[index] >= 0 ? dp[index + 1, rest - arr[index]] : 0);
        return dp[0, aim];
    }

    // 为了测试
    private static int[] RandomArray(int maxLen, int maxValue)
    {
        var n = (int)(Utility.GetRandomDouble * maxLen);
        var arr = new int[n];
        for (var i = 0; i < n; i++) arr[i] = (int)(Utility.GetRandomDouble * maxValue) + 1;
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
        var maxLen = 20;
        var maxValue = 30;
        var testTime = 1000000;

        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var arr = RandomArray(maxLen, maxValue);
            var aim = (int)(Utility.GetRandomDouble * maxValue);
            var ans1 = CoinWays(arr, aim);
            var ans2 = Dp(arr, aim);
            if (ans1 != ans2)
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr);
                Console.WriteLine(aim);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}