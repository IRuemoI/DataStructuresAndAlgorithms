namespace AdvancedTraining.Lesson50;

public class MaximumVacationDays //Problem_0568
{
    private static int MaxVacationDays(int[][] fly, int[][] day)
    {
        var n = fly.Length;
        var k = day[0].Length;
        // pas[i] = {a, b, c}
        // 从a、b、c能飞到i
        var pass = new int[n][];
        for (var i = 0; i < n; i++)
        {
            var s = 0;
            for (var j = 0; j < n; j++)
                if (fly[j][i] != 0)
                    s++;
            pass[i] = new int[s];
            for (var j = n - 1; j >= 0; j--)
                if (fly[j][i] != 0)
                    pass[i][--s] = j;
        }

        // dp[i][j] -> 第i周必须在j这座城，0~i-1周（随意），最大休假天数
        var dp = new int[k][];
        // 飞的时机，是周一早上飞，认为对时间没有影响，直接到某个城，然后过一周
        dp[0][0] = day[0][0];
        for (var j = 1; j < n; j++) dp[0][j] = fly[0][j] != 0 ? day[j][0] : -1;
        for (var i = 1; i < k; i++)
            // 第i周
        for (var j = 0; j < n; j++)
        {
            // 在j号城过！
            // 第i周，要怎么到j号城
            // 下面max的初始值，我第i-1周，就在j号城，选择不动地方，进入第i周
            var max = dp[i - 1][j];
            foreach (var p in pass[j])
                // 枚举什么？能到j号城的城市p
                max = Math.Max(max, dp[i - 1][p]);
            dp[i][j] = max != -1 ? max + day[j][i] : -1;
        }

        var ans = 0;
        for (var i = 0; i < n; i++) ans = Math.Max(ans, dp[k - 1][i]);
        return ans;
    }
}