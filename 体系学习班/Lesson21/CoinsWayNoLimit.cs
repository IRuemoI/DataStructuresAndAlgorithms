//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson21;

public class CoinsWayNoLimit
{
    private static int CoinsWay(int[]? arr, int aim)
    {
        if (arr == null || arr.Length == 0 || aim < 0) return 0;

        return Process(arr, 0, aim);
    }

    // arr[index....] 所有的面值，每一个面值都可以任意选择张数，组成正好rest这么多钱，方法数多少？
    private static int Process(int[] arr, int index, int rest)
    {
        if (index == arr.Length)
            // 没钱了
            return rest == 0 ? 1 : 0;

        var ways = 0;
        for (var zhang = 0; zhang * arr[index] <= rest; zhang++)
            ways += Process(arr, index + 1, rest - zhang * arr[index]);

        return ways;
    }

    private static int Dp1(int[]? arr, int aim)
    {
        if (arr == null || arr.Length == 0 || aim < 0) return 0;

        var n = arr.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 1;
        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
        {
            var ways = 0;
            for (var zhang = 0; zhang * arr[index] <= rest; zhang++) ways += dp[index + 1, rest - zhang * arr[index]];

            dp[index, rest] = ways;
        }

        return dp[0, aim];
    }

    private static int Dp2(int[]? arr, int aim)
    {
        if (arr == null || arr.Length == 0 || aim < 0) return 0;

        var n = arr.Length;
        var dp = new int[n + 1, aim + 1];
        dp[n, 0] = 1;
        for (var index = n - 1; index >= 0; index--)
        for (var rest = 0; rest <= aim; rest++)
        {
            dp[index, rest] = dp[index + 1, rest];
            if (rest - arr[index] >= 0) dp[index, rest] += dp[index, rest - arr[index]];
        }

        return dp[0, aim];
    }

    // 为了测试
    private static int[] RandomArray(int maxLen, int maxValue)
    {
        var n = (int)(Utility.getRandomDouble * maxLen);
        var arr = new int[n];
        var has = new bool[maxValue + 1];
        for (var i = 0; i < n; i++)
        {
            do
            {
                arr[i] = (int)(Utility.getRandomDouble * maxValue) + 1;
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
        var maxLen = 10;
        var maxValue = 30;
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
}