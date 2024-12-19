//pass
namespace AdvancedTraining.Lesson11;

// 本题测试链接 : https://leetcode.cn/problems/palindrome-partitioning-ii/
public class PalindromePartitioningIi
{
    // 测试链接只测了本题的第一问，直接提交可以通过
    private static int MinCut(string s)
    {
        if (ReferenceEquals(s, null) || s.Length < 2) return 0;
        var str = s.ToCharArray();
        var n = str.Length;
        var checkMap = createCheckMap(str, n);
        var dp = new int[n + 1];
        dp[n] = 0;
        for (var i = n - 1; i >= 0; i--)
            if (checkMap[i, n - 1])
            {
                dp[i] = 1;
            }
            else
            {
                var next = int.MaxValue;
                for (var j = i; j < n; j++)
                    if (checkMap[i, j])
                        next = Math.Min(next, dp[j + 1]);
                dp[i] = 1 + next;
            }

        return dp[0] - 1;
    }

    private static bool[,] createCheckMap(char[] str, int n)
    {
        var ans = new bool[n, n];
        for (var i = 0; i < n - 1; i++)
        {
            ans[i, i] = true;
            ans[i, i + 1] = str[i] == str[i + 1];
        }

        ans[n - 1, n - 1] = true;
        for (var i = n - 3; i >= 0; i--)
        for (var j = i + 2; j < n; j++)
            ans[i, j] = str[i] == str[j] && ans[i + 1, j - 1];
        return ans;
    }

    // 本题第二问，返回其中一种结果
    private static IList<string> minCutOneWay(string s)
    {
        IList<string> ans = new List<string>();
        if (ReferenceEquals(s, null) || s.Length < 2)
        {
            ans.Add(s);
        }
        else
        {
            var str = s.ToCharArray();
            var n = str.Length;
            var checkMap = createCheckMap(str, n);
            var dp = new int[n + 1];
            dp[n] = 0;
            for (var i = n - 1; i >= 0; i--)
                if (checkMap[i, n - 1])
                {
                    dp[i] = 1;
                }
                else
                {
                    var next = int.MaxValue;
                    for (var j = i; j < n; j++)
                        if (checkMap[i, j])
                            next = Math.Min(next, dp[j + 1]);
                    dp[i] = 1 + next;
                }

            // dp[i]  (0....5) 回文！  dp[0] == dp[6] + 1
            //  (0....5)   6
            for (int i = 0, j = 1; j <= n; j++)
                if (checkMap[i, j - 1] && dp[i] == dp[j] + 1)
                {
                    ans.Add(s.Substring(i, j - i));
                    i = j;
                }
        }

        return ans;
    }

    // 本题第三问，返回所有结果
    private static IList<IList<string>> minCutAllWays(string s)
    {
        var ans = new List<IList<string>>();
        if (ReferenceEquals(s, null) || s.Length < 2)
        {
            var cur = new List<string> { s };
            ans.Add(cur);
        }
        else
        {
            var str = s.ToCharArray();
            var n = str.Length;
            var checkMap = createCheckMap(str, n);
            var dp = new int[n + 1];
            dp[n] = 0;
            for (var i = n - 1; i >= 0; i--)
                if (checkMap[i, n - 1])
                {
                    dp[i] = 1;
                }
                else
                {
                    var next = int.MaxValue;
                    for (var j = i; j < n; j++)
                        if (checkMap[i, j])
                            next = Math.Min(next, dp[j + 1]);
                    dp[i] = 1 + next;
                }

            Process(s, 0, 1, checkMap, dp, new List<string>(), ans);
        }

        return ans;
    }

    // s[0....i-1]  存到path里去了
    // s[i..j-1]考察的分出来的第一份
    private static void Process(string s, int i, int j, bool[,] checkMap, int[] dp, IList<string> path,
        IList<IList<string>> ans)
    {
        if (j == s.Length)
        {
            // s[i...N-1]
            if (checkMap[i, j - 1] && dp[i] == dp[j] + 1)
            {
                path.Add(s.Substring(i, j - i));
                ans.Add(copyStringList(path));
                path.RemoveAt(path.Count - 1);
            }
        }
        else
        {
            // s[i...j-1]
            if (checkMap[i, j - 1] && dp[i] == dp[j] + 1)
            {
                path.Add(s.Substring(i, j - i));
                Process(s, j, j + 1, checkMap, dp, path, ans);
                path.RemoveAt(path.Count - 1);
            }

            Process(s, i, j + 1, checkMap, dp, path, ans);
        }
    }

    private static IList<string> copyStringList(IList<string> list)
    {
        IList<string> ans = new List<string>();
        foreach (var str in list) ans.Add(str);
        return ans;
    }

    public static void Run()
    {
        Console.WriteLine("本题第二问，返回其中一种结果测试开始");
        var s = "abacbc";
        var ans2 = minCutOneWay(s);
        foreach (var str in ans2) Console.Write(str + " ");
        Console.WriteLine();

        s = "aabccbac";
        ans2 = minCutOneWay(s);
        foreach (var str in ans2) Console.Write(str + " ");
        Console.WriteLine();

        s = "aabaa";
        ans2 = minCutOneWay(s);
        foreach (var str in ans2) Console.Write(str + " ");
        Console.WriteLine();
        Console.WriteLine("本题第二问，返回其中一种结果测试结束");
        Console.WriteLine();
        Console.WriteLine("本题第三问，返回所有可能结果测试开始");
        s = "cbbbcbc";
        var ans3 = minCutAllWays(s);
        foreach (var way in ans3)
        {
            foreach (var str in way) Console.Write(str + " ");
            Console.WriteLine();
        }

        Console.WriteLine();

        s = "aaaaaa";
        ans3 = minCutAllWays(s);
        foreach (var way in ans3)
        {
            foreach (var str in way) Console.Write(str + " ");
            Console.WriteLine();
        }

        Console.WriteLine();

        s = "fcfffcffcc";
        ans3 = minCutAllWays(s);
        foreach (var way in ans3)
        {
            foreach (var str in way) Console.Write(str + " ");
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("本题第三问，返回所有可能结果测试结束");
    }
}