#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson16;

public class SmallestUnFormedSum
{
    private static int UnformedSum1(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 1;
        var set = new HashSet<int>();
        Process(arr, 0, 0, set);
        var min = int.MaxValue;
        for (var i = 0; i != arr.Length; i++) min = Math.Min(min, arr[i]);
        for (var i = min + 1; i != int.MinValue; i++)
            if (!set.Contains(i))
                return i;
        return 0;
    }

    private static void Process(int[] arr, int i, int sum, HashSet<int> set)
    {
        if (i == arr.Length)
        {
            set.Add(sum);
            return;
        }

        Process(arr, i + 1, sum, set);
        Process(arr, i + 1, sum + arr[i], set);
    }

    private static int UnformedSum2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 1;
        var sum = 0;
        var min = int.MaxValue;
        for (var i = 0; i != arr.Length; i++)
        {
            sum += arr[i];
            min = Math.Min(min, arr[i]);
        }

        // boolean[,] dp ...
        var n = arr.Length;
        var dp = new bool [n, sum + 1];
        for (var i = 0; i < n; i++)
            // arr[0..i] 0
            dp[i, 0] = true;
        dp[0, arr[0]] = true;
        for (var i = 1; i < n; i++)
        for (var j = 1; j <= sum; j++)
            dp[i, j] = dp[i - 1, j] || (j - arr[i] >= 0 && dp[i - 1, j - arr[i]]);
        for (var j = min; j <= sum; j++)
            if (!dp[n - 1, j])
                return j;
        return sum + 1;
    }

    // 已知arr中肯定有1这个数
    private static int UnformedSum3(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        Array.Sort(arr); // O (N * logN)
        var range = 1;
        // arr[0] == 1
        for (var i = 1; i != arr.Length; i++)
            if (arr[i] > range + 1)
                return range + 1;
            else
                range += arr[i];
        return range + 1;
    }

    private static int[] GenerateArray(int len, int maxValue)
    {
        var res = new int[len];
        for (var i = 0; i != res.Length; i++) res[i] = (int)(Utility.GetRandomDouble * maxValue) + 1;
        return res;
    }

    private static void PrintArray(int[] arr)
    {
        for (var i = 0; i != arr.Length; i++) Console.Write(arr[i] + " ");
        Console.WriteLine();
    }

    public static void Run()
    {
        var len = 27;
        var max = 30;
        var arr = GenerateArray(len, max);
        PrintArray(arr);

        Utility.RestartStopwatch();
        Console.WriteLine(UnformedSum1(arr));
        Console.WriteLine("cost time: " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
        Console.WriteLine("======================================");

        Utility.RestartStopwatch();
        Console.WriteLine(UnformedSum2(arr));
        Console.WriteLine("cost time: " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
        Console.WriteLine("======================================");

        Console.WriteLine("set arr[0] to 1");
        arr[0] = 1;
        Utility.RestartStopwatch();
        Console.WriteLine(UnformedSum3(arr));

        Console.WriteLine("cost time: " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
    }
}