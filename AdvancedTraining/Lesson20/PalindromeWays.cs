//pass
#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson20;

public class PalindromeWays
{
    private static int Ways1(string str)
    {
        if (ReferenceEquals(str, null) || str.Length == 0) return 0;
        var s = str.ToCharArray();
        var path = new char[s.Length];
        return Process(str.ToCharArray(), 0, path, 0);
    }

    private static int Process(char[] s, int si, char[] path, int pi)
    {
        if (si == s.Length) return IsP(path, pi) ? 1 : 0;
        var ans = Process(s, si + 1, path, pi);
        path[pi] = s[si];
        ans += Process(s, si + 1, path, pi + 1);
        return ans;
    }

    private static bool IsP(char[] path, int pi)
    {
        if (pi == 0) return false;
        var l = 0;
        var r = pi - 1;
        while (l < r)
            if (path[l++] != path[r--])
                return false;
        return true;
    }

    private static int Ways2(string str)
    {
        if (ReferenceEquals(str, null) || str.Length == 0) return 0;
        var s = str.ToCharArray();
        var n = s.Length;
        var dp = new int[n, n];
        for (var i = 0; i < n; i++) dp[i, i] = 1;
        for (var i = 0; i < n - 1; i++) dp[i, i + 1] = s[i] == s[i + 1] ? 3 : 2;
        for (var l = n - 3; l >= 0; l--)
        for (var r = l + 2; r < n; r++)
        {
            dp[l, r] = dp[l + 1, r] + dp[l, r - 1] - dp[l + 1, r - 1];
            if (s[l] == s[r]) dp[l, r] += dp[l + 1, r - 1] + 1;
        }

        return dp[0, n - 1];
    }

    private static string RandomString(int len, int types)
    {
        var str = new char[len];
        for (var i = 0; i < str.Length; i++) str[i] = (char)('a' + (int)(Utility.getRandomDouble * types));
        return new string(str);
    }

    public static void Run()
    {
        const int n = 10;
        var types = 5;
        var testTimes = 5000;
        Console.WriteLine("测试开始");
        for (var i = 0; i < testTimes; i++)
        {
            var len = (int)(Utility.getRandomDouble * n);
            var str = RandomString(len, types);
            var ans1 = Ways1(str);
            var ans2 = Ways2(str);
            if (ans1 != ans2)
            {
                Console.WriteLine(str);
                Console.WriteLine(ans1);
                Console.WriteLine(ans2);
                Console.WriteLine("出错啦！");
                break;
            }
        }

        Console.WriteLine("测试结束");
    }
}