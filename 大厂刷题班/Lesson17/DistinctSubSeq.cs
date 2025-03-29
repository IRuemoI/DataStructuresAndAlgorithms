//pass

namespace AdvancedTraining.Lesson17;

// 测试链接 : https://leetcode-cn.com/problems/21dk04/
public class DistinctSubSeq
{
    private static int NumDistinct1(string str1, string str2)
    {
        var s = str1.ToCharArray();
        var t = str2.ToCharArray();
        return Process(s, t, s.Length, t.Length);
    }

    private static int Process(char[] s, char[] t, int i, int j)
    {
        if (j == 0) return 1;
        if (i == 0) return 0;
        var res = Process(s, t, i - 1, j);
        if (s[i - 1] == t[j - 1]) res += Process(s, t, i - 1, j - 1);
        return res;
    }

    // S[...i]的所有子序列中，包含多少个字面值等于T[...j]这个字符串的子序列
    // 记为dp[i,j]
    // 可能性1）S[...i]的所有子序列中，都不以s[i]结尾，则dp[i,j]肯定包含dp[i-1,j]
    // 可能性2）S[...i]的所有子序列中，都必须以s[i]结尾，
    // 这要求S[i] == T[j]，则dp[i,j]包含dp[i-1,j-1]
    private static int NumDistinct2(string str1, string str2)
    {
        var s = str1.ToCharArray();
        var t = str2.ToCharArray();
        // Dp[i,j] : s[0..i] T[0...j]

        // Dp[i,j] : s只拿前i个字符做子序列，有多少个子序列，字面值等于T的前j个字符的前缀串
        var dp = new int[s.Length + 1, t.Length + 1];
        // Dp[0,0]
        // Dp[0,j] = s只拿前0个字符做子序列, T前j个字符
        for (var j = 0; j <= t.Length; j++) dp[0, j] = 0;
        for (var i = 0; i <= s.Length; i++) dp[i, 0] = 1;
        for (var i = 1; i <= s.Length; i++)
        for (var j = 1; j <= t.Length; j++)
            dp[i, j] = dp[i - 1, j] + (s[i - 1] == t[j - 1] ? dp[i - 1, j - 1] : 0);
        return dp[s.Length, t.Length];
    }

    private static int NumDistinct3(string str1, string str2)
    {
        var s = str1.ToCharArray();
        var t = str2.ToCharArray();
        var dp = new int[t.Length + 1];
        dp[0] = 1;
        for (var j = 1; j <= t.Length; j++) dp[j] = 0;
        for (var i = 1; i <= s.Length; i++)
        for (var j = t.Length; j >= 1; j--)
            dp[j] += s[i - 1] == t[j - 1] ? dp[j - 1] : 0;
        return dp[t.Length];
    }

    private static int Dp(string str1, string str2)
    {
        var s = str1.ToCharArray();
        var t = str2.ToCharArray();
        var n = s.Length;
        var m = t.Length;
        var dp = new int[n, m];
        // s[0..0] T[0..0] Dp[0,0]
        dp[0, 0] = s[0] == t[0] ? 1 : 0;
        for (var i = 1; i < n; i++) dp[i, 0] = s[i] == t[0] ? dp[i - 1, 0] + 1 : dp[i - 1, 0];
        for (var i = 1; i < n; i++)
        for (var j = 1; j <= Math.Min(i, m - 1); j++)
        {
            dp[i, j] = dp[i - 1, j];
            if (s[i] == t[j]) dp[i, j] += dp[i - 1, j - 1];
        }

        return dp[n - 1, m - 1];
    }

    public static void Run()
    {
        Console.WriteLine(NumDistinct1("1212311112121132", "13"));
        Console.WriteLine(NumDistinct2("1212311112121132", "13"));
        Console.WriteLine(NumDistinct3("1212311112121132", "13"));
        Console.WriteLine(Dp("1212311112121132", "13"));
    }
}