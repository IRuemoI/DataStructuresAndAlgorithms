//测试通过

namespace Algorithms.Lesson39;

public class SnacksWays
{
    private static int Ways1(int[] arr, int w)
    {
        // arr[0...]
        return Process(arr, 0, w);
    }

    // 从左往右的经典模型
    // 还剩的容量是rest，arr[index...]自由选择，
    // 返回选择方案
    // index ： 0～N
    // rest : 0~w
    private static int Process(int[] arr, int index, int rest)
    {
        if (rest < 0)
            // 没有容量了
            // -1 无方案的意思
            return -1;
        // rest>=0,
        if (index == arr.Length)
            // 无零食可选
            return 1;
        // rest >=0
        // 有零食index
        // index号零食，要 or 不要
        // index, rest
        // (index+1, rest)
        // (index+1, rest-arr[i])
        var next1 = Process(arr, index + 1, rest); // 不要
        var next2 = Process(arr, index + 1, rest - arr[index]); // 要
        return next1 + (next2 == -1 ? 0 : next2);
    }

    private static int Ways2(int[] arr, int w)
    {
        var n = arr.Length;
        var dp = new int[n + 1, w + 1]; //删除RectangularArrays

        for (var j = 0; j <= w; j++) dp[n, j] = 1;
        for (var i = n - 1; i >= 0; i--)
        for (var j = 0; j <= w; j++)
            dp[i, j] = dp[i + 1, j] + (j - arr[i] >= 0 ? dp[i + 1, j - arr[i]] : 0);
        return dp[0, w];
    }

    private static int Ways3(int[] arr, int w)
    {
        var n = arr.Length;
        var dp = new int[n, w + 1];
        for (var i = 0; i < n; i++) dp[i, 0] = 1;
        if (arr[0] <= w) dp[0, arr[0]] = 1;
        for (var i = 1; i < n; i++)
        for (var j = 1; j <= w; j++)
            dp[i, j] = dp[i - 1, j] + (j - arr[i] >= 0 ? dp[i - 1, j - arr[i]] : 0);
        var ans = 0;
        for (var j = 0; j <= w; j++) ans += dp[n - 1, j];
        return ans;
    }

    public static void Run()
    {
        int[] arr = [4, 3, 2, 9];
        const int w = 8;
        Console.WriteLine(Ways1(arr, w));
        Console.WriteLine(Ways2(arr, w));
        Console.WriteLine(Ways3(arr, w));
    }
}