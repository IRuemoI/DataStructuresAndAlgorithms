//pass
namespace AdvancedTraining.Lesson30;

public class DecodeWays //leetcode_0091
{
    private static int NumDecode1(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var str = s.ToCharArray();
        return Process(str, 0);
    }

    // 潜台词：str[0...index-1]已经转化完了，不用操心了
    // str[index....] 能转出多少有效的，返回方法数
    private static int Process(char[] str, int index)
    {
        if (index == str.Length) return 1;
        if (str[index] == '0') return 0;
        var ways = Process(str, index + 1);
        if (index + 1 == str.Length) return ways;
        var num = (str[index] - '0') * 10 + str[index + 1] - '0';
        if (num < 27) ways += Process(str, index + 2);
        return ways;
    }

    private static int NumDecode2(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var str = s.ToCharArray();
        var n = str.Length;
        // dp[i] -> Process(str, index)返回值 index 0 ~ N
        var dp = new int[n + 1];
        dp[n] = 1;

        // dp依次填好 dp[i] dp[i+1] dp[i+2]
        for (var i = n - 1; i >= 0; i--)
            if (str[i] != '0')
            {
                dp[i] = dp[i + 1];
                if (i + 1 == str.Length) continue;
                var num = (str[i] - '0') * 10 + str[i + 1] - '0';
                if (num <= 26) dp[i] += dp[i + 2];
            }

        return dp[0];
    }

    private static int NumDecode3(string s)
    {
        if (ReferenceEquals(s, null) || s.Length == 0) return 0;
        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int[n + 1];
        dp[n] = 1;
        for (var i = n - 1; i >= 0; i--)
            if (str[i] == '0')
            {
                dp[i] = 0;
            }
            else if (str[i] == '1')
            {
                dp[i] = dp[i + 1];
                if (i + 1 < n) dp[i] += dp[i + 2];
            }
            else if (str[i] == '2')
            {
                dp[i] = dp[i + 1];
                if (i + 1 < str.Length && str[i + 1] >= '0' && str[i + 1] <= '6') dp[i] += dp[i + 2];
            }
            else
            {
                dp[i] = dp[i + 1];
            }

        return dp[0];
    }

    public static void Run()
    {
        Console.WriteLine(NumDecode1("226")); //输出3
        Console.WriteLine(NumDecode2("226")); //输出3
        Console.WriteLine(NumDecode3("226")); //输出3
    }
}