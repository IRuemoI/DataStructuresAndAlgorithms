//通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson19;

// 这个问题leetcode上可以直接测
// 链接：https://leetcode.cn/problems/longest-common-subsequence/
public static class LongestCommonSubsequence
{
    private static int LongestCommonSubsequence1(string s1, string s2)
    {
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return 0;

        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        // 尝试
        return Process1(str1, str2, str1.Length - 1, str2.Length - 1);
    }

    private static int Process1(char[] str1, char[] str2, int i, int j)
    {
        if (i == 0 && j == 0) return str1[i] == str2[j] ? 1 : 0;

        if (i == 0) //如果字符串1只剩下一个字符
            return str1[i] == str2[j] ? 1 : Process1(str1, str2, i, j - 1);

        if (j == 0) //如果字符串2只剩下一个字符
            return str1[i] == str2[j] ? 1 : Process1(str1, str2, i - 1, j);

        // i != 0 && j != 0
        // 三种情况
        // 字符串1的最后一个字符不被考虑
        // 字符串2的最后一个字符不被考虑
        // 字符串1和字符串2的最后一个字符都不被考虑
        var p1 = Process1(str1, str2, i - 1, j);
        var p2 = Process1(str1, str2, i, j - 1);
        var p3 = str1[i] == str2[j] ? 1 + Process1(str1, str2, i - 1, j - 1) : 0;
        return Math.Max(p1, Math.Max(p2, p3));
    }

    private static int LongestCommonSubsequence2(string s1, string s2)
    {
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return 0;

        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        var n = str1.Length;
        var m = str2.Length;
        var dp = new int[n, m];
        dp[0, 0] = str1[0] == str2[0] ? 1 : 0;
        for (var j = 1; j < m; j++) dp[0, j] = str1[0] == str2[j] ? 1 : dp[0, j - 1];

        for (var i = 1; i < n; i++) dp[i, 0] = str1[i] == str2[0] ? 1 : dp[i - 1, 0];

        for (var i = 1; i < n; i++)
        for (var j = 1; j < m; j++)
        {
            var p1 = dp[i - 1, j];
            var p2 = dp[i, j - 1];
            var p3 = str1[i] == str2[j] ? 1 + dp[i - 1, j - 1] : 0;
            dp[i, j] = Math.Max(p1, Math.Max(p2, p3));
        }

        return dp[n - 1, m - 1];
    }

    public static void Run()
    {
        const string? text1A = "abcde", text2A = "ace"; //3
        const string? text1B = "abc", text2B = "abc"; //3
        const string? text1C = "abc", text2C = "def"; //0


        Console.WriteLine($"参数1:{text1A},参数2:{text2A}----------");
        Utility.RestartStopwatch();
        Console.WriteLine("方法1结果:" + LongestCommonSubsequence1(text1A, text2A) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法2结果:" + LongestCommonSubsequence2(text1A, text2A) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");

        Console.WriteLine($"参数1:{text1B},参数2:{text2B}----------");
        Utility.RestartStopwatch();
        Console.WriteLine("方法1结果:" + LongestCommonSubsequence1(text1B, text2B) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法2结果:" + LongestCommonSubsequence2(text1B, text2B) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");

        Console.WriteLine($"参数1:{text1C},参数2:{text2C}----------");
        Utility.RestartStopwatch();
        Console.WriteLine("方法1结果:" + LongestCommonSubsequence1(text1C, text2C) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine("方法2结果:" + LongestCommonSubsequence2(text1C, text2C) + ",耗时:" +
                          Utility.GetStopwatchElapsedMilliseconds() + "ms");
    }
}