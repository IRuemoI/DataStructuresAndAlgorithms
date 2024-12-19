namespace AdvancedTraining.Lesson51;

//pass
public class UncrossedLines //leetcode 1035
{
    // 针对这个题的题意，做的动态规划
    private static int MaxUncrossedLines1(int[]? a1, int[]? b1)
    {
        if (a1 == null || a1.Length == 0 || b1 == null || b1.Length == 0) return 0;
        var n = a1.Length;
        var m = b1.Length;
        // dp[i,j]代表: A[0...i]对应B[0...j]最多能划几条线
        var dp = new int[n, m];
        if (a1[0] == b1[0]) dp[0, 0] = 1;
        for (var j = 1; j < m; j++) dp[0, j] = a1[0] == b1[j] ? 1 : dp[0, j - 1];
        for (var i = 1; i < n; i++) dp[i, 0] = a1[i] == b1[0] ? 1 : dp[i - 1, 0];
        // 某个值(key)，上次在A中出现的位置(value)
        var aValueLastIndex = new Dictionary<int, int>
        {
            [a1[0]] = 0
        };
        // 某个值(key)，上次在B中出现的位置(value)
        var bValueLastIndex = new Dictionary<int, int>();
        for (var i = 1; i < n; i++)
        {
            aValueLastIndex[a1[i]] = i;
            bValueLastIndex[b1[0]] = 0;
            for (var j = 1; j < m; j++)
            {
                bValueLastIndex[b1[j]] = j;
                // 可能性1，就是不让A[i]去划线
                var p1 = dp[i - 1, j];
                // 可能性2，就是不让B[j]去划线
                var p2 = dp[i, j - 1];
                // 可能性3，就是要让A[i]去划线，那么如果A[i]==5，它跟谁划线？
                // 贪心的点：一定是在B[0...j]中，尽量靠右侧的5
                var p3 = 0;
                if (bValueLastIndex.ContainsKey(a1[i]))
                {
                    var last = bValueLastIndex[a1[i]];
                    p3 = (last > 0 ? dp[i - 1, last - 1] : 0) + 1;
                }

                // 可能性4，就是要让B[j]去划线，那么如果B[j]==7，它跟谁划线？
                // 贪心的点：一定是在A[0...i]中，尽量靠右侧的7
                var p4 = 0;
                if (aValueLastIndex.ContainsKey(b1[j]))
                {
                    var last = aValueLastIndex[b1[j]];
                    p4 = (last > 0 ? dp[last - 1, j - 1] : 0) + 1;
                }

                dp[i, j] = Math.Max(Math.Max(p1, p2), Math.Max(p3, p4));
            }

            bValueLastIndex.Clear();
        }

        return dp[n - 1, m - 1];
    }

    // 但是其实这个题，不就是求两个数组的最长公共子序列吗？
    private static int MaxUncrossedLines2(int[]? a1, int[]? b1)
    {
        if (a1 == null || a1.Length == 0 || b1 == null || b1.Length == 0) return 0;
        var n = a1.Length;
        var m = b1.Length;
        var dp = new int[n, m];
        dp[0, 0] = a1[0] == b1[0] ? 1 : 0;
        for (var j = 1; j < m; j++) dp[0, j] = a1[0] == b1[j] ? 1 : dp[0, j - 1];
        for (var i = 1; i < n; i++) dp[i, 0] = a1[i] == b1[0] ? 1 : dp[i - 1, 0];
        for (var i = 1; i < n; i++)
        for (var j = 1; j < m; j++)
        {
            var p1 = dp[i - 1, j];
            var p2 = dp[i, j - 1];
            var p3 = a1[i] == b1[j] ? 1 + dp[i - 1, j - 1] : 0;
            dp[i, j] = Math.Max(p1, Math.Max(p2, p3));
        }

        return dp[n - 1, m - 1];
    }
}