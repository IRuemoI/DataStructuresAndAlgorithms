#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson39;

// 给定一个非负数组arr，和一个正数m。 返回arr的所有子序列中累加和%m之后的最大值。
public class SubsequenceMaxModM
{
    private static int Max1(int[] arr, int m)
    {
        var set = new HashSet<int>();
        Process(arr, 0, 0, set);
        var max = 0;
        foreach (var sum in set) max = Math.Max(max, sum % m);

        return max;
    }

    private static void Process(int[] arr, int index, int sum, HashSet<int> set)
    {
        if (index == arr.Length)
        {
            set.Add(sum);
        }
        else
        {
            Process(arr, index + 1, sum, set);
            Process(arr, index + 1, sum + arr[index], set);
        }
    }

    private static int Max2(int[] arr, int m)
    {
        var sum = 0;
        var n = arr.Length;
        for (var i = 0; i < n; i++) sum += arr[i];
        var dp = new bool[n, sum + 1];
        for (var i = 0; i < n; i++) dp[i, 0] = true;

        dp[0, arr[0]] = true;
        for (var i = 1; i < n; i++)
        for (var j = 1; j <= sum; j++)
        {
            dp[i, j] = dp[i - 1, j];
            if (j - arr[i] >= 0) dp[i, j] |= dp[i - 1, j - arr[i]];
        }

        var ans = 0;
        for (var j = 0; j <= sum; j++)
            if (dp[n - 1, j])
                ans = Math.Max(ans, j % m);

        return ans;
    }

    private static int Max3(int[] arr, int m)
    {
        var n = arr.Length;
        // 0...m-1
        var dp = new bool[n, m];
        for (var i = 0; i < n; i++) dp[i, 0] = true;

        dp[0, arr[0] % m] = true;
        for (var i = 1; i < n; i++)
        for (var j = 1; j < m; j++)
        {
            // dp[i][j] T or F
            dp[i, j] = dp[i - 1, j];
            var cur = arr[i] % m;
            if (cur <= j)
                dp[i, j] |= dp[i - 1, j - cur];
            else
                dp[i, j] |= dp[i - 1, m + j - cur];
        }

        var ans = 0;
        for (var i = 0; i < m; i++)
            if (dp[n - 1, i])
                ans = i;

        return ans;
    }

    // 如果arr的累加和很大，m也很大
    // 但是arr的长度相对不大
    private static int Max4(int[] arr, int m)
    {
        if (arr.Length == 1) return arr[0] % m;

        var mid = (arr.Length - 1) / 2;
        var sortSet1 = new SortedSet<int>();
        Process4(arr, 0, 0, mid, m, sortSet1);
        var sortSet2 = new SortedSet<int>();
        Process4(arr, mid + 1, 0, arr.Length - 1, m, sortSet2);
        var ans = 0;
        foreach (var leftMod in sortSet1)
            ans = Math.Max(ans, leftMod + sortSet2.LastOrDefault(x => x <= m - 1 - leftMod)); //逻辑正确，但是存在性能问题

        return ans;
    }

    // 从index出发，最后有边界是end+1，arr[index...end]
    private static void Process4(int[] arr, int index, int sum, int end, int m, SortedSet<int> sortSet)
    {
        if (index == end + 1)
        {
            sortSet.Add(sum % m);
        }
        else
        {
            Process4(arr, index + 1, sum, end, m, sortSet);
            Process4(arr, index + 1, sum + arr[index], end, m, sortSet);
        }
    }

    private static int[] GenerateRandomArray(int len, int value)
    {
        var ans = new int[(int)(Utility.getRandomDouble * len) + 1];
        for (var i = 0; i < ans.Length; i++) ans[i] = (int)(Utility.getRandomDouble * value);

        return ans;
    }

    public static void Run()
    {
        var len = 10;
        var value = 100;
        var m = 76;
        var testTime = 1000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTime; i++)
        {
            var arr = GenerateRandomArray(len, value);
            var ans1 = Max1(arr, m);
            var ans2 = Max2(arr, m);
            var ans3 = Max3(arr, m);
            var ans4 = Max4(arr, m);
            if (ans1 != ans2 || ans2 != ans3 || ans3 != ans4)
            {
                Console.WriteLine("出错啦！");
                Console.WriteLine($"ans1:{ans1},ans2:{ans2},ans3:{ans3},ans4:{ans4}");
            }
        }

        Console.WriteLine("测试完成");
    }
}