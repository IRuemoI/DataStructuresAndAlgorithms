//pass

namespace AdvancedTraining.Lesson30;

public class DecodeWaysIi //leetcode_0639
{
    private const long Mod = 1000000007;

    private static int NumDecode0(string str)
    {
        return F(str.ToCharArray(), 0);
    }

    private static int F(char[] str, int i)
    {
        if (i == str.Length) return 1;
        if (str[i] == '0') return 0;
        // str[index]有字符且不是'0'
        if (str[i] != '*')
        {
            // str[index] = 1~9
            // i -> 单转
            var p1 = F(str, i + 1);
            if (i + 1 == str.Length) return p1;
            if (str[i + 1] != '*')
            {
                var num = (str[i] - '0') * 10 + str[i + 1] - '0';
                var p2 = 0;
                if (num < 27) p2 = F(str, i + 2);
                return p1 + p2;
            }
            else
            {
                // str[i+1] == '*'
                // i i+1 -> 一起转 1* 2* 3* 9*
                var p2 = 0;
                if (str[i] < '3') p2 = F(str, i + 2) * (str[i] == '1' ? 9 : 6);
                return p1 + p2;
            }
        }
        else
        {
            // str[i] == '*' 1~9
            // i 单转 9种
            var p1 = 9 * F(str, i + 1);
            if (i + 1 == str.Length) return p1;
            if (str[i + 1] != '*')
            {
                // * 0 10 20
                // * 1 11 21
                // * 2 12 22
                // * 3 13 23
                // * 6 16 26
                // * 7 17
                // * 8 18
                // * 9 19
                var p2 = (str[i + 1] < '7' ? 2 : 1) * F(str, i + 2);
                return p1 + p2;
            }
            else
            {
                // str[i+1] == *
                // **
                // 11~19 9
                // 21 ~26 6
                // 15
                var p2 = 15 * F(str, i + 2);
                return p1 + p2;
            }
        }
    }

    private static int NumDecode1(string str)
    {
        var dp = new long[str.Length];
        return (int)Ways1(str.ToCharArray(), 0, dp);
    }

    private static long Ways1(char[] s, int i, long[] dp)
    {
        if (i == s.Length) return 1;
        if (s[i] == '0') return 0;
        if (dp[i] != 0) return dp[i];
        var ans = Ways1(s, i + 1, dp) * (s[i] == '*' ? 9 : 1);
        if (s[i] == '1' || s[i] == '2' || s[i] == '*')
            if (i + 1 < s.Length)
            {
                if (s[i + 1] == '*')
                {
                    ans += Ways1(s, i + 2, dp) * (s[i] == '*' ? 15 : s[i] == '1' ? 9 : 6);
                }
                else
                {
                    if (s[i] == '*')
                        ans += Ways1(s, i + 2, dp) * (s[i + 1] < '7' ? 2 : 1);
                    else
                        ans += (s[i] - '0') * 10 + s[i + 1] - '0' < 27 ? Ways1(s, i + 2, dp) : 0;
                }
            }

        ans %= Mod;
        dp[i] = ans;
        return ans;
    }

    private static int NumDecode2(string str)
    {
        var s = str.ToCharArray();
        var n = s.Length;
        var dp = new long[n + 1];
        dp[n] = 1;
        for (var i = n - 1; i >= 0; i--)
            if (s[i] != '0')
            {
                dp[i] = dp[i + 1] * (s[i] == '*' ? 9 : 1);
                if (s[i] == '1' || s[i] == '2' || s[i] == '*')
                    if (i + 1 < n)
                    {
                        if (s[i + 1] == '*')
                        {
                            dp[i] += dp[i + 2] * (s[i] == '*' ? 15 : s[i] == '1' ? 9 : 6);
                        }
                        else
                        {
                            if (s[i] == '*')
                                dp[i] += dp[i + 2] * (s[i + 1] < '7' ? 2 : 1);
                            else
                                dp[i] += (s[i] - '0') * 10 + s[i + 1] - '0' < 27 ? dp[i + 2] : 0;
                        }
                    }

                dp[i] %= Mod;
            }

        return (int)dp[0];
    }

    private static int NumDecode3(string str)
    {
        var s = str.ToCharArray();
        var n = s.Length;
        long a = 1;
        long b = 1;
        long c = 0;
        for (var i = n - 1; i >= 0; i--)
        {
            if (s[i] != '0')
            {
                c = b * (s[i] == '*' ? 9 : 1);
                if (s[i] == '1' || s[i] == '2' || s[i] == '*')
                    if (i + 1 < n)
                    {
                        if (s[i + 1] == '*')
                        {
                            c += a * (s[i] == '*' ? 15 : s[i] == '1' ? 9 : 6);
                        }
                        else
                        {
                            if (s[i] == '*')
                                c += a * (s[i + 1] < '7' ? 2 : 1);
                            else
                                c += a * ((s[i] - '0') * 10 + s[i + 1] - '0' < 27 ? 1 : 0);
                        }
                    }
            }

            c %= Mod;
            a = b;
            b = c;
            c = 0;
        }

        return (int)b;
    }

    public static void Run()
    {
        Console.WriteLine(NumDecode0("1*")); //输出18
        Console.WriteLine(NumDecode1("1*")); //输出18
        Console.WriteLine(NumDecode2("1*")); //输出18
        Console.WriteLine(NumDecode3("1*")); //输出18
    }
}