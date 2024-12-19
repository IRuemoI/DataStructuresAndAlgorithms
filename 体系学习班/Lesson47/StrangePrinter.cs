//测试通过

namespace Algorithms.Lesson47;

// 测试链接 : https://leetcode.cn/problems/strange-printer/
public class StrangePrinter
{
    private static int StrangePrinter1(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var str = s.ToCharArray();
        return Process1(str, 0, str.Length - 1);
    }

    // 要想刷出str[L...R]的样子！
    // 返回最少的转数
    private static int Process1(char[] str, int l, int r)
    {
        if (l == r) return 1;
        // L...R
        var ans = r - l + 1;
        for (var k = l + 1; k <= r; k++)
            // L...k-1 k....R
            ans = Math.Min(ans, Process1(str, l, k - 1) + Process1(str, k, r) - (str[l] == str[k] ? 1 : 0));
        return ans;
    }

    private static int StrangePrinter2(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int[n, n];
        return Process2(str, 0, n - 1, dp);
    }

    private static int Process2(char[] str, int l, int r, int[,] dp)
    {
        if (dp[l, r] != 0) return dp[l, r];
        var ans = r - l + 1;
        if (l == r)
            ans = 1;
        else
            for (var k = l + 1; k <= r; k++)
                ans = Math.Min(ans, Process2(str, l, k - 1, dp) + Process2(str, k, r, dp) - (str[l] == str[k] ? 1 : 0));
        dp[l, r] = ans;
        return ans;
    }

    private static int StrangePrinter3(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int [n, n];
        dp[n - 1, n - 1] = 1;
        for (var i = 0; i < n - 1; i++)
        {
            dp[i, i] = 1;
            dp[i, i + 1] = str[i] == str[i + 1] ? 1 : 2;
        }

        for (var l = n - 3; l >= 0; l--)
        for (var r = l + 2; r < n; r++)
        {
            dp[l, r] = r - l + 1;
            for (var k = l + 1; k <= r; k++)
                dp[l, r] = Math.Min(dp[l, r], dp[l, k - 1] + dp[k, r] - (str[l] == str[k] ? 1 : 0));
        }

        return dp[0, n - 1];
    }

    public static void Run()
    {
        const string str1 = "aaabbb";
        const string str2 = "aba";

        Console.WriteLine(StrangePrinter1(str1));
        Console.WriteLine(StrangePrinter2(str1));
        Console.WriteLine(StrangePrinter3(str1));
        Console.WriteLine("----------------");
        Console.WriteLine(StrangePrinter1(str2));
        Console.WriteLine(StrangePrinter2(str2));
        Console.WriteLine(StrangePrinter3(str2));
    }
}