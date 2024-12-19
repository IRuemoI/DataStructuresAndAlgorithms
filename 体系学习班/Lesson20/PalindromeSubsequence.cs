//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson20;

// 测试链接：https://leetcode.cn/problems/longest-palindromic-subsequence/
public static class PalindromeSubsequence
{
    //范围尝试模型，暴力递归
    private static int Code1(string? s)
    {
        if (string.IsNullOrEmpty(s)) return 0;

        var str = s.ToCharArray();
        return F(str, 0, str.Length - 1);
    }

    // str[L..R]最长回文子序列长度返回
    private static int F(char[] str, int l, int r)
    {
        if (l == r) return 1; //如果区间上只剩下一个字符，回文长度为1

        if (l == r - 1) return str[l] == str[r] ? 2 : 1; //如果区间上剩下两个字符，如果两个字符相同，回文长度为2，否则长度为1

        var p1 = F(str, l + 1, r - 1); //如果去掉区间两端的字符可以构成回文
        var p2 = F(str, l, r - 1); //如果去掉区间右端的字符可以构成回文
        var p3 = F(str, l + 1, r); //如果去掉区间左端的字符可以构成回文
        var p4 = str[l] != str[r] ? 0 : 2 + F(str, l + 1, r - 1); //如果区间两端字符相同，则回文长度为2+去掉区间两端字符后可以构成回文的长度
        return Math.Max(Math.Max(p1, p2), Math.Max(p3, p4));
    }

    //范围尝试模型，动态规划
    private static int Code1Dp(string s)
    {
        if (string.IsNullOrEmpty(s)) return 0;

        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int[n, n];
        dp[n - 1, n - 1] = 1;
        for (var i = 0; i < n - 1; i++)
        {
            dp[i, i] = 1;
            dp[i, i + 1] = str[i] == str[i + 1] ? 2 : 1;
        }

        for (var l = n - 3; l >= 0; l--)
        for (var r = l + 2; r < n; r++)
        {
            //优化前
            //var p1 = dp[l + 1, r - 1];//被优化掉了
            //var p2 = dp[l, r - 1];
            //var p3 = dp[l + 1, r];
            //var p4 = str[l] != str[r] ? 0 : 2 + dp[l + 1, r - 1];

            //优化掉左下的依赖，用左和下两个依赖填写dp[l, r]
            dp[l, r] = Math.Max(dp[l, r - 1], dp[l + 1, r]);
            if (str[l] == str[r]) dp[l, r] = Math.Max(dp[l, r], 2 + dp[l + 1, r - 1]);
        }

        return dp[0, n - 1];
    }

    //反转字符串求最长公共子序列的方式求解(样本对应模型):暴力递归
    private static int Code2(string? s)
    {
        if (string.IsNullOrEmpty(s)) return 0;

        if (s.Length == 1) return 1;

        var str = s.ToCharArray();
        var reverse = Reverse(str);
        return Process(str, reverse);
    }

    private static char[] Reverse(char[] str)
    {
        var n = str.Length;
        var reverse = new char[str.Length];
        foreach (var element in str) reverse[--n] = element;

        return reverse;
    }

    private static int Process(char[] str1, char[] str2)
    {
        var n = str1.Length;
        var m = str2.Length;
        var dp = new int[n, m];
        dp[0, 0] = str1[0] == str2[0] ? 1 : 0;
        for (var i = 1; i < n; i++) dp[i, 0] = str1[i] == str2[0] ? 1 : dp[i - 1, 0];

        for (var j = 1; j < m; j++) dp[0, j] = str1[0] == str2[j] ? 1 : dp[0, j - 1];

        for (var i = 1; i < n; i++)
        for (var j = 1; j < m; j++)
        {
            dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
            if (str1[i] == str2[j]) dp[i, j] = Math.Max(dp[i, j], dp[i - 1, j - 1] + 1);
        }

        return dp[n - 1, m - 1];
    }

    //反转字符串求最长公共子序列的方式求解(样本对应模型):动态规划
    private static int Code2Dp(string s)
    {
        if (string.IsNullOrEmpty(s)) return 0;

        if (s.Length == 1) return 1;

        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int[n, n];
        dp[n - 1, n - 1] = 1;
        for (var i = 0; i < n - 1; i++)
        {
            dp[i, i] = 1;
            dp[i, i + 1] = str[i] == str[i + 1] ? 2 : 1;
        }

        for (var i = n - 3; i >= 0; i--)
        for (var j = i + 2; j < n; j++)
        {
            dp[i, j] = Math.Max(dp[i, j - 1], dp[i + 1, j]);
            if (str[i] == str[j]) dp[i, j] = Math.Max(dp[i, j], dp[i + 1, j - 1] + 2);
        }

        return dp[0, n - 1];
    }

    public static void Run()
    {
        const string str1 = "bbbab"; //4
        const string str2 = "cbbd"; //2

        Console.WriteLine("参数1:" + str1 + " ----------");
        Utility.RestartStopwatch();
        Console.WriteLine("方法1结果:" + Code1(str1) + ",耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法2结果:" + Code1Dp(str1) + ",耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法3结果:" + Code2(str1) + ",耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法3结果:" + Code2Dp(str1) + ",耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");

        Console.WriteLine("参数1:" + str2 + " ----------");
        Utility.RestartStopwatch();
        Console.WriteLine("方法1结果:" + Code1(str2) + ",耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法2结果:" + Code1Dp(str2) + ",耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法3结果:" + Code2(str2) + ",耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法3结果:" + Code2Dp(str2) + ",耗时:" + Utility.GetStopwatchElapsedMilliseconds() + "ms");
    }
}