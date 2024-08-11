namespace AdvancedTraining.Lesson04;

// 本题测试链接 : https://leetcode.cn/problems/interleaving-string/
public class InterleavingString
{
    private static bool IsInterleave(string s1, string s2, string s3)
    {
        if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null) || ReferenceEquals(s3, null)) return false;
        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        var str3 = s3.ToCharArray();
        if (str3.Length != str1.Length + str2.Length) return false;
        var dp = new bool[str1.Length + 1, str2.Length + 1];
        dp[0, 0] = true;
        for (var i = 1; i <= str1.Length; i++)
        {
            if (str1[i - 1] != str3[i - 1]) break;
            dp[i, 0] = true;
        }

        for (var j = 1; j <= str2.Length; j++)
        {
            if (str2[j - 1] != str3[j - 1]) break;
            dp[0, j] = true;
        }

        for (var i = 1; i <= str1.Length; i++)
        for (var j = 1; j <= str2.Length; j++)
            if ((str1[i - 1] == str3[i + j - 1] && dp[i - 1, j]) || (str2[j - 1] == str3[i + j - 1] && dp[i, j - 1]))
                dp[i, j] = true;
        return dp[str1.Length, str2.Length];
    }


    public static void Run()
    {
        Console.WriteLine(IsInterleave("aabcc", "dbbca", "aadbbcbcac")); //输出true
    }
}