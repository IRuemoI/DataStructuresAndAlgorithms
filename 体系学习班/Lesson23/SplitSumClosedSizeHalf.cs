//测试通过

namespace Algorithms.Lesson23;

public class SplitSumClosedSizeHalf
{
    private static int Right(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        var sum = 0;
        foreach (var num in arr) sum += num;

        if ((arr.Length & 1) == 0)
            return Process(arr, 0, arr.Length / 2, sum / 2);
        return Math.Max(Process(arr, 0, arr.Length / 2, sum / 2), Process(arr, 0, arr.Length / 2 + 1, sum / 2));
    }

    // arr[i....]自由选择，挑选的个数一定要是picks个，累加和<=rest, 离rest最近的返回
    private static int Process(int[] arr, int i, int picks, int rest)
    {
        if (i == arr.Length) return picks == 0 ? 0 : -1;

        var p1 = Process(arr, i + 1, picks, rest);
        // 就是要使用arr[i]这个数
        var p2 = -1;
        var next = -1;
        if (arr[i] <= rest) next = Process(arr, i + 1, picks - 1, rest - arr[i]);

        if (next != -1) p2 = arr[i] + next;

        return Math.Max(p1, p2);
    }

    private static int Dp(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        var sum = 0;
        foreach (var num in arr) sum += num;

        sum /= 2;
        var n = arr.Length;
        var m = (n + 1) / 2;
        var dp = new int[n + 1, m + 1, sum + 1];
        for (var i = 0; i <= n; i++)
        for (var j = 0; j <= m; j++)
        for (var k = 0; k <= sum; k++)
            dp[i, j, k] = -1;

        for (var rest = 0; rest <= sum; rest++) dp[n, 0, rest] = 0;

        for (var i = n - 1; i >= 0; i--)
        for (var picks = 0; picks <= m; picks++)
        for (var rest = 0; rest <= sum; rest++)
        {
            var p1 = dp[i + 1, picks, rest];
            // 就是要使用arr[i]这个数
            var p2 = -1;
            var next = -1;
            if (picks - 1 >= 0 && arr[i] <= rest) next = dp[i + 1, picks - 1, rest - arr[i]];

            if (next != -1) p2 = arr[i] + next;

            dp[i, picks, rest] = Math.Max(p1, p2);
        }

        if ((arr.Length & 1) == 0)
            return dp[0, arr.Length / 2, sum];
        return Math.Max(dp[0, arr.Length / 2, sum], dp[0, arr.Length / 2 + 1, sum]);
    }


    private static int Dp2(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;

        var sum = 0;
        foreach (var num in arr) sum += num;

        sum >>= 1;
        var n = arr.Length;
        var m = (arr.Length + 1) >> 1;
        var dp = new int[n, m + 1, sum + 1];
        for (var i = 0; i < n; i++)
        for (var j = 0; j <= m; j++)
        for (var k = 0; k <= sum; k++)
            dp[i, j, k] = int.MinValue;

        for (var i = 0; i < n; i++)
        for (var k = 0; k <= sum; k++)
            dp[i, 0, k] = 0;

        for (var k = 0; k <= sum; k++) dp[0, 1, k] = arr[0] <= k ? arr[0] : int.MinValue;

        for (var i = 1; i < n; i++)
        for (var j = 1; j <= Math.Min(i + 1, m); j++)
        for (var k = 0; k <= sum; k++)
        {
            dp[i, j, k] = dp[i - 1, j, k];
            if (k - arr[i] >= 0) dp[i, j, k] = Math.Max(dp[i, j, k], dp[i - 1, j - 1, k - arr[i]] + arr[i]);
        }

        return Math.Max(dp[n - 1, m, sum], dp[n - 1, n - m, sum]);
    }

    //用于测试
    private static int[] RandomArray(int len, int value)
    {
        var arr = new int[len];
        for (var i = 0; i < arr.Length; i++) arr[i] = (int)(new Random().NextDouble() * value);

        return arr;
    }

    //用于测试
    private static void PrintArray(int[] arr)
    {
        foreach (var num in arr) Console.Write(num + " ");

        Console.WriteLine();
    }

    //用于测试
    public static void Run()
    {
        var maxLen = 10;
        var maxValue = 50;
        var testTime = 10000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var len = (int)(new Random().NextDouble() * maxLen);
            var arr = RandomArray(len, maxValue);
            var ans1 = Right(arr);
            var ans2 = Dp(arr);
            var ans3 = Dp2(arr);
            if (ans1 != ans2 || ans1 != ans3)
            {
                PrintArray(arr);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine(ans3);
                Console.WriteLine("出错啦！");
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}