namespace AdvancedTraining.Lesson52;

public class CoinPath //leetcode_0656
{
    // arr 0 -> n-1
    // arr[i] = -1 死了！
    private static int MinCost(int[]? arr, int jump)
    {
        if (arr == null || arr.Length == 0) return 0;
        var n = arr.Length;
        if (arr[0] == -1 || arr[n - 1] == -1) return -1;
        // dp[i] : 从0位置开始出发，到达i位置的最小代价
        var dp = new int[n];
        dp[0] = arr[0];
        for (var i = 1; i < n; i++)
        {
            dp[i] = int.MaxValue;
            if (arr[i] != -1)
                for (var pre = Math.Max(0, i - jump); pre < i; pre++)
                    if (dp[pre] != -1)
                        dp[i] = Math.Min(dp[i], dp[pre] + arr[i]);
            dp[i] = dp[i] == int.MaxValue ? -1 : dp[i];
        }

        return dp[n - 1];
    }

    private static IList<int> CheapestJump(int[] arr, int jump)
    {
        var n = arr.Length;
        var best = new int[n];
        var last = new int[n];
        var size = new int[n];
        Array.Fill(best, int.MaxValue);
        Array.Fill(last, -1);
        best[0] = 0;
        for (var i = 0; i < n; i++)
            if (arr[i] != -1)
                for (var j = Math.Max(0, i - jump); j < i; j++)
                    if (arr[j] != -1)
                    {
                        var cur = best[j] + arr[i];
                        // 1) 代价低换方案！
                        // 2) 代价一样，但是点更多，换方案！
                        if (cur < best[i] || (cur == best[i] && size[i] - 1 < size[j]))
                        {
                            best[i] = cur;
                            last[i] = j;
                            size[i] = size[j] + 1;
                        }
                    }

        IList<int> path = new List<int>();
        for (var cur = n - 1; cur >= 0; cur = last[cur]) path.Insert(0, cur + 1);
        return path[0] != 1 ? new List<int>() : path;
    }

    public static void Run()
    {
        var arr = new[] { 1, 2, 4, -1, 2 };
        var jump = 2;
        var res = CheapestJump(arr, jump);
        Console.WriteLine(string.Join(",", res)); //[1,3,5]
    }
}