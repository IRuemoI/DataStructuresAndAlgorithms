//pass
namespace AdvancedTraining.Lesson12;

// 测试链接 : https://leetcode.cn/problems/regular-expression-matching/
public class RegularExpressionMatch
{
    private static bool IsValid(char[] s, char[] e)
    {
        // s中不能有'.' or '*'
        foreach (var item in s)
            if (item == '*' || item == '.')
                return false;

        // 开头的e[0]不能是'*'，没有相邻的'*'
        for (var i = 0; i < e.Length; i++)
            if (e[i] == '*' && (i == 0 || e[i - 1] == '*'))
                return false;

        return true;
    }

    // 初始尝试版本，不包含斜率优化
    private static bool IsMatch1(string str, string exp)
    {
        if (ReferenceEquals(str, null) || ReferenceEquals(exp, null)) return false;
        var s = str.ToCharArray();
        var e = exp.ToCharArray();
        return IsValid(s, e) && Process(s, e, 0, 0);
    }

    // str[si.....] 能不能被 exp[ei.....]配出来！ true false
    private static bool Process(char[] s, char[] e, int si, int ei)
    {
        if (ei == e.Length)
            // exp 没了 str？
            return si == s.Length;
        // exp[ei]还有字符
        // ei + 1位置的字符，不是*
        if (ei + 1 == e.Length || e[ei + 1] != '*')
            // ei + 1 不是*
            // str[si] 必须和 exp[ei] 能配上！
            return si != s.Length && (e[ei] == s[si] || e[ei] == '.') && Process(s, e, si + 1, ei + 1);
        // exp[ei]还有字符
        // ei + 1位置的字符，是*!
        while (si != s.Length && (e[ei] == s[si] || e[ei] == '.'))
        {
            if (Process(s, e, si, ei + 2)) return true;
            si++;
        }

        return Process(s, e, si, ei + 2);
    }

    // 改记忆化搜索+斜率优化
    private static bool IsMatch2(string str, string exp)
    {
        if (ReferenceEquals(str, null) || ReferenceEquals(exp, null)) return false;
        var s = str.ToCharArray();
        var e = exp.ToCharArray();
        if (!IsValid(s, e)) return false;
        var dp = new int[s.Length + 1, e.Length + 1];
        // dp[i,j] = 0, 没算过！
        // dp[i,j] = -1 算过，返回值是false
        // dp[i,j] = 1 算过，返回值是true
        return IsValid(s, e) && Process2(s, e, 0, 0, dp);
    }

    private static bool Process2(char[] s, char[] e, int si, int ei, int[,] dp)
    {
        if (dp[si, ei] != 0) return dp[si, ei] == 1;
        bool ans;
        if (ei == e.Length)
        {
            ans = si == s.Length;
        }
        else
        {
            if (ei + 1 == e.Length || e[ei + 1] != '*')
            {
                ans = si != s.Length && (e[ei] == s[si] || e[ei] == '.') && Process2(s, e, si + 1, ei + 1, dp);
            }
            else
            {
                if (si == s.Length)
                {
                    // ei ei+1 *
                    ans = Process2(s, e, si, ei + 2, dp);
                }
                else
                {
                    // si没结束
                    if (s[si] != e[ei] && e[ei] != '.')
                        ans = Process2(s, e, si, ei + 2, dp);
                    else
                        // s[si] 可以和 e[ei]配上
                        ans = Process2(s, e, si, ei + 2, dp) || Process2(s, e, si + 1, ei, dp);
                }
            }
        }

        dp[si, ei] = ans ? 1 : -1;
        return ans;
    }

    // 动态规划版本 + 斜率优化
    private static bool IsMatch3(string str, string pattern)
    {
        if (ReferenceEquals(str, null) || ReferenceEquals(pattern, null)) return false;
        var s = str.ToCharArray();
        var p = pattern.ToCharArray();
        if (!IsValid(s, p)) return false;
        var n = s.Length;
        var m = p.Length;
        var dp = new bool[n + 1, m + 1];
        dp[n, m] = true;
        for (var j = m - 1; j >= 0; j--) dp[n, j] = j + 1 < m && p[j + 1] == '*' && dp[n, j + 2];
        // dp[0..N-2,M-1]都等于false，只有dp[N-1,M-1]需要讨论
        if (n > 0 && m > 0) dp[n - 1, m - 1] = s[n - 1] == p[m - 1] || p[m - 1] == '.';
        for (var i = n - 1; i >= 0; i--)
        for (var j = m - 2; j >= 0; j--)
            if (p[j + 1] != '*')
            {
                dp[i, j] = (s[i] == p[j] || p[j] == '.') && dp[i + 1, j + 1];
            }
            else
            {
                if ((s[i] == p[j] || p[j] == '.') && dp[i + 1, j])
                    dp[i, j] = true;
                else
                    dp[i, j] = dp[i, j + 2];
            }

        return dp[0, 0];
    }

    public static void Run()
    {
        string s = "ab", p = ".*";

        Console.WriteLine(IsMatch1(s, p));
        Console.WriteLine(IsMatch2(s, p));
        Console.WriteLine(IsMatch3(s, p));
    }
}