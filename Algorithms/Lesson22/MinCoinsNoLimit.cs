//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson22;

public class MinCoinsNoLimit
{
    private static int MinCoins(int[] arr, int aim)
    {
        return Process(arr, 0, aim);
    }

    // arr[index...]面值，每种面值张数自由选择，
    // 搞出rest正好这么多钱，返回最小张数
    // 拿Integer.MAX_VALUE标记怎么都搞定不了
    private static int Process(int[] arr, int index, int rest)
    {
        if (index == arr.Length) return rest == 0 ? 0 : int.MaxValue;

        var ans = int.MaxValue;
        for (var zhang = 0; zhang * arr[index] <= rest; zhang++)
        {
            var next = Process(arr, index + 1, rest - zhang * arr[index]);
            if (next != int.MaxValue) ans = Math.Min(ans, zhang + next);
        }

        return ans;
    }

    private static int Dp1(int[] arr, int aim)
    {
        if (aim == 0) return 0;

        var n = arr.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 0;
        for (var j = 1; j <= aim; j++) dp[n, j] = int.MaxValue;

        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
        {
            var ans = int.MaxValue;
            for (var zhang = 0; zhang * arr[index] <= rest; zhang++)
            {
                var next = dp[index + 1, rest - zhang * arr[index]];
                if (next != int.MaxValue) ans = Math.Min(ans, zhang + next);
            }

            dp[index, rest] = ans;
        }

        return dp[0, aim];
    }

    private static int Dp2(int[] arr, int aim)
    {
        if (aim == 0) return 0;

        var n = arr.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 0;
        for (var j = 1; j <= aim; j++) dp[n, j] = int.MaxValue;

        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
        {
            dp[index, rest] = dp[index + 1, rest];
            if (rest - arr[index] >= 0
                && dp[index, rest - arr[index]] != int.MaxValue)
                dp[index, rest] = Math.Min(dp[index, rest], dp[index, rest - arr[index]] + 1);
        }

        return dp[0, aim];
    }

    // 为了测试
    private static int[] RandomArray(int maxLen, int maxValue)
    {
        var n = (int)(Utility.GetRandomDouble * maxLen);
        var arr = new int[n];
        var has = new bool[maxValue + 1];
        for (var i = 0; i < n; i++)
        {
            do
            {
                arr[i] = (int)(Utility.GetRandomDouble * maxValue) + 1;
            } while (has[arr[i]]);

            has[arr[i]] = true;
        }

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
        var testTime = 300000;

        Console.WriteLine("功能测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var n = (int)(Utility.GetRandomDouble * maxLen);
            var arr = RandomArray(n, maxValue);
            var aim = (int)(Utility.GetRandomDouble * maxValue);
            var ans1 = MinCoins(arr, aim);
            var ans2 = Dp1(arr, aim);
            var ans3 = Dp2(arr, aim);
            if (ans1 != ans2 || ans1 != ans3)
            {
                Console.WriteLine("出错啦！");
                PrintArray(arr);
                Console.WriteLine(aim);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                break;
            }
        }

        Console.WriteLine("功能测试结束");
    }
}