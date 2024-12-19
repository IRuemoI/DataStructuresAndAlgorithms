//测试通过
namespace Algorithms.Lesson19;

public class ConvertToLetterString
{
    // str只含有数字字符0~9
    // 返回多少种转化方案
    private static int Number(string? str)
    {
        if (string.IsNullOrEmpty(str)) return 0;

        return Process(str.ToCharArray(), 0);
    }

    // str[0..i-1]转化无需过问
    // str[i.....]去转化，返回有多少种转化方法
    private static int Process(char[] str, int i)
    {
        if (i == str.Length) return 1;

        // i没到最后，说明有字符
        if (str[i] == '0')
            // 之前的决定有问题
            return 0;

        // str[i] != '0'
        // 可能性一，i单转
        var ways = Process(str, i + 1);
        if (i + 1 < str.Length && (str[i] - '0') * 10 + str[i + 1] - '0' < 27) ways += Process(str, i + 2);

        return ways;
    }

    private static int Dp(string? s)
    {
        if (string.IsNullOrEmpty(s)) return 0;

        var str = s.ToCharArray();
        var n = str.Length;
        var dp = new int[n + 1];
        dp[n] = 1;
        for (var i = n - 1; i >= 0; i--)
            if (str[i] != '0')
            {
                var ways = dp[i + 1];
                if (i + 1 < str.Length && (str[i] - '0') * 10 + str[i + 1] - '0' < 27) ways += dp[i + 2];

                dp[i] = ways;
            }

        return dp[0];
    }

    public static void Run()
    {
        Console.WriteLine(Number("7210231231232031203123"));
        Console.WriteLine(Dp("7210231231232031203123"));
    }
}