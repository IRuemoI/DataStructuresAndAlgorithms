//测试通过

namespace Algorithms.Lesson23;

public class SplitSumClosed
{
    private static int Right(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        var sum = 0;
        foreach (var num in arr) sum += num;

        return Process(arr, 0, sum / 2);
    }

    // arr[i...]可以自由选择，请返回累加和尽量接近rest，但不能超过rest的情况下，最接近的累加和是多少？
    private static int Process(int[] arr, int i, int rest)
    {
        if (i == arr.Length) return 0;

        // 还有数，arr[i]这个数
        // 可能性1，不使用arr[i]
        var p1 = Process(arr, i + 1, rest);
        // 可能性2，要使用arr[i]
        var p2 = 0;
        if (arr[i] <= rest) p2 = arr[i] + Process(arr, i + 1, rest - arr[i]);

        return Math.Max(p1, p2);
    }

    private static int Dp(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        var sum = 0;
        foreach (var num in arr) sum += num;

        sum /= 2;
        var n = arr.Length;
        var dp = new int[n + 1, sum + 1];
        for (var i = n - 1; i >= 0; i--)
        for (var rest = 0; rest <= sum; rest++)
        {
            // 可能性1，不使用arr[i]
            var p1 = dp[i + 1, rest];
            // 可能性2，要使用arr[i]
            var p2 = 0;
            if (arr[i] <= rest) p2 = arr[i] + dp[i + 1, rest - arr[i]];

            dp[i, rest] = Math.Max(p1, p2);
        }

        return dp[0, sum];
    }

    private static int[] RandomArray(int len, int value)
    {
        var arr = new int[len];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)(new Random().NextDouble() * value);

        return arr;
    }

    private static void PrintArray(int[] arr)
    {
        foreach (var num in arr) Console.Write(num + " ");

        Console.WriteLine();
    }

    public static void Run()
    {
        var maxLen = 20;
        var maxValue = 50;
        var testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(new Random().NextDouble() * maxLen);
            var arr = RandomArray(len, maxValue);
            var ans1 = Right(arr);
            var ans2 = Dp(arr);
            if (ans1 != ans2)
            {
                PrintArray(arr);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine("出错啦！");
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}