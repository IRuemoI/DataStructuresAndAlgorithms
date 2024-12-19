//pass
namespace AdvancedTraining.Lesson05;

public class EditCost
{
    private static int MinCost1(string s1, string s2, int ic, int dc, int rc)
    {
        if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null)) return 0;
        var str1 = s1.ToCharArray();
        var str2 = s2.ToCharArray();
        var n = str1.Length + 1;
        var m = str2.Length + 1;
        var dp = new int [n, m];
        // dp[0,0] = 0
        for (var i = 1; i < n; i++) dp[i, 0] = dc * i;
        for (var j = 1; j < m; j++) dp[0, j] = ic * j;
        for (var i = 1; i < n; i++)
        for (var j = 1; j < m; j++)
        {
            dp[i, j] = dp[i - 1, j - 1] + (str1[i - 1] == str2[j - 1] ? 0 : rc);
            dp[i, j] = Math.Min(dp[i, j], dp[i, j - 1] + ic);
            dp[i, j] = Math.Min(dp[i, j], dp[i - 1, j] + dc);
        }

        return dp[n - 1, m - 1];
    }

    private static int MinCost2(string str1, string str2, int ic, int dc, int rc)
    {
        if (ReferenceEquals(str1, null) || ReferenceEquals(str2, null)) return 0;
        var chs1 = str1.ToCharArray();
        var chs2 = str2.ToCharArray();
        var longs = chs1.Length >= chs2.Length ? chs1 : chs2;
        var shorts = chs1.Length < chs2.Length ? chs1 : chs2;
        if (chs1.Length < chs2.Length) (ic, dc) = (dc, ic);

        var dp = new int[shorts.Length + 1];
        for (var i = 1; i <= shorts.Length; i++) dp[i] = ic * i;
        for (var i = 1; i <= longs.Length; i++)
        {
            var pre = dp[0];
            dp[0] = dc * i;
            for (var j = 1; j <= shorts.Length; j++)
            {
                var tmp = dp[j];
                if (longs[i - 1] == shorts[j - 1])
                    dp[j] = pre;
                else
                    dp[j] = pre + rc;
                dp[j] = Math.Min(dp[j], dp[j - 1] + ic);
                dp[j] = Math.Min(dp[j], tmp + dc);
                pre = tmp;
            }
        }

        return dp[shorts.Length];
    }

    public static void Run()
    {
        var str1 = "ab12cd3";
        var str2 = "abcdf";
        Console.WriteLine(MinCost1(str1, str2, 5, 3, 2));
        Console.WriteLine(MinCost2(str1, str2, 5, 3, 2));

        str1 = "abcdf";
        str2 = "ab12cd3";
        Console.WriteLine(MinCost1(str1, str2, 3, 2, 4));
        Console.WriteLine(MinCost2(str1, str2, 3, 2, 4));

        str1 = "";
        str2 = "ab12cd3";
        Console.WriteLine(MinCost1(str1, str2, 1, 7, 5));
        Console.WriteLine(MinCost2(str1, str2, 1, 7, 5));

        str1 = "abcdf";
        str2 = "";
        Console.WriteLine(MinCost1(str1, str2, 2, 9, 8));
        Console.WriteLine(MinCost2(str1, str2, 2, 9, 8));
    }
}