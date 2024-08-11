namespace AdvancedTraining.Lesson11;

// 本题测试链接 : https://leetcode.cn/problems/minimum-insertion-steps-to-make-a-string-palindrome/
public class MinimumInsertionStepsToMakeAStringPalindrome
{
    // 测试链接只测了本题的第一问，直接提交可以通过
    private static int MinInsertions(string s)
    {
        if (ReferenceEquals(s, null) || s.Length < 2) return 0;
        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int[n, n];
        for (var i = 0; i < n - 1; i++) dp[i, i + 1] = str[i] == str[i + 1] ? 0 : 1;
        for (var i = n - 3; i >= 0; i--)
        for (var j = i + 2; j < n; j++)
        {
            dp[i, j] = Math.Min(dp[i, j - 1], dp[i + 1, j]) + 1;
            if (str[i] == str[j]) dp[i, j] = Math.Min(dp[i, j], dp[i + 1, j - 1]);
        }

        return dp[0, n - 1];
    }

    // 本题第二问，返回其中一种结果
    private static string MinInsertionsOneWay(string s)
    {
        if (ReferenceEquals(s, null) || s.Length < 2) return s;
        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int[n, n];
        for (var i = 0; i < n - 1; i++) dp[i, i + 1] = str[i] == str[i + 1] ? 0 : 1;
        for (var i = n - 3; i >= 0; i--)
        for (var j = i + 2; j < n; j++)
        {
            dp[i, j] = Math.Min(dp[i, j - 1], dp[i + 1, j]) + 1;
            if (str[i] == str[j]) dp[i, j] = Math.Min(dp[i, j], dp[i + 1, j - 1]);
        }

        var l = 0;
        var r = n - 1;
        var ans = new char[n + dp[l, r]];
        var lAns = 0;
        var rAns = ans.Length - 1;
        while (l < r)
            if (dp[l, r - 1] == dp[l, r] - 1)
            {
                ans[lAns++] = str[r];
                ans[rAns--] = str[r--];
            }
            else if (dp[l + 1, r] == dp[l, r] - 1)
            {
                ans[lAns++] = str[l];
                ans[rAns--] = str[l++];
            }
            else
            {
                ans[lAns++] = str[l++];
                ans[rAns--] = str[r--];
            }

        if (l == r) ans[lAns] = str[l];
        return new string(ans);
    }

    // 本题第三问，返回所有可能的结果
    private static IList<string> minInsertionsAllWays(string s)
    {
        var ans = new List<string>();
        if (ReferenceEquals(s, null) || s.Length < 2)
        {
            if (s != null) ans.Add(s);
        }
        else
        {
            var str = s.ToCharArray();
            var n = str.Length;
            var dp = new int[n, n];
            for (var i = 0; i < n - 1; i++) dp[i, i + 1] = str[i] == str[i + 1] ? 0 : 1;
            for (var i = n - 3; i >= 0; i--)
            for (var j = i + 2; j < n; j++)
            {
                dp[i, j] = Math.Min(dp[i, j - 1], dp[i + 1, j]) + 1;
                if (str[i] == str[j]) dp[i, j] = Math.Min(dp[i, j], dp[i + 1, j - 1]);
            }

            var m = n + dp[0, n - 1];
            var path = new char[m];
            Process(str, dp, 0, n - 1, path, 0, m - 1, ans);
        }

        return ans;
    }

    // 当前来到的动态规划中的格子，(L,R)
    // path ....  [pl....pr] ....
    private static void Process(char[] str, int[,] dp, int l, int r, char[] path, int pl, int pr, IList<string> ans)
    {
        if (l >= r)
        {
            // L > R  L==R
            if (l == r) path[pl] = str[l];
            ans.Add(new string(path));
        }
        else
        {
            if (dp[l, r - 1] == dp[l, r] - 1)
            {
                path[pl] = str[r];
                path[pr] = str[r];
                Process(str, dp, l, r - 1, path, pl + 1, pr - 1, ans);
            }

            if (dp[l + 1, r] == dp[l, r] - 1)
            {
                path[pl] = str[l];
                path[pr] = str[l];
                Process(str, dp, l + 1, r, path, pl + 1, pr - 1, ans);
            }

            if (str[l] == str[r] && (l == r - 1 || dp[l + 1, r - 1] == dp[l, r]))
            {
                path[pl] = str[l];
                path[pr] = str[r];
                Process(str, dp, l + 1, r - 1, path, pl + 1, pr - 1, ans);
            }
        }
    }

    public static void Run()
    {
        Console.WriteLine("本题第二问，返回其中一种结果测试开始");
        var s = "mbadm";
        var ans2 = MinInsertionsOneWay(s);
        Console.WriteLine(ans2);

        s = "leetcode";
        ans2 = MinInsertionsOneWay(s);
        Console.WriteLine(ans2);

        s = "aabaa";
        ans2 = MinInsertionsOneWay(s);
        Console.WriteLine(ans2);
        Console.WriteLine("本题第二问，返回其中一种结果测试结束");

        Console.WriteLine();

        Console.WriteLine("本题第三问，返回所有可能的结果测试开始");
        s = "mbadm";
        var ans3 = minInsertionsAllWays(s);
        foreach (var way in ans3) Console.WriteLine(way);
        Console.WriteLine();

        s = "leetcode";
        ans3 = minInsertionsAllWays(s);
        foreach (var way in ans3) Console.WriteLine(way);
        Console.WriteLine();

        s = "aabaa";
        ans3 = minInsertionsAllWays(s);
        foreach (var way in ans3) Console.WriteLine(way);
        Console.WriteLine();
        Console.WriteLine("本题第三问，返回所有可能的结果测试结束");
    }
}