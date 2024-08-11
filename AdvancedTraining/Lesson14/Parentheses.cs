namespace AdvancedTraining.Lesson14;

public class Parentheses
{
    private static bool Valid(string s)
    {
        var str = s.ToCharArray();
        var count = 0;
        foreach (var item in str)
        {
            count += item == '(' ? 1 : -1;
            if (count < 0) return false;
        }

        return count == 0;
    }

    private static int NeedParentheses(string s)
    {
        var str = s.ToCharArray();
        var count = 0;
        var need = 0;
        foreach (var item in str)
            if (item == '(')
            {
                count++;
            }
            else
            {
                // 遇到的是')'
                if (count == 0)
                    need++;
                else
                    count--;
            }

        return count + need;
    }

    private static bool IsValid(char[]? str)
    {
        if (str == null || str.Length == 0) return false;
        var status = 0;
        foreach (var item in str)
        {
            if (item != ')' && item != '(') return false;
            if (item == ')' && --status < 0) return false;
            if (item == '(') status++;
        }

        return status == 0;
    }

    private static int Deep(string s)
    {
        var str = s.ToCharArray();
        if (!IsValid(str)) return 0;
        var count = 0;
        var max = 0;
        foreach (var item in str)
            if (item == '(')
                max = Math.Max(max, ++count);
            else
                count--;

        return max;
    }

    // s只由(和)组成
    // 求最长有效括号子串长度
    // 本题测试链接 : https://leetcode.cn/problems/longest-Valid-parentheses/
    private static int LongestValidParentheses(string s)
    {
        if (ReferenceEquals(s, null) || s.Length < 2) return 0;
        var str = s.ToCharArray();
        // dp[i] : 子串必须以i位置结尾的情况下，往左最远能扩出多长的有效区域
        var dp = new int[str.Length];
        // dp[0] = 0; （  ）
        var ans = 0;
        for (var i = 1; i < str.Length; i++)
        {
            if (str[i] == ')')
            {
                // 当前谁和i位置的)，去配！
                var pre = i - dp[i - 1] - 1;
                if (pre >= 0 && str[pre] == '(') dp[i] = dp[i - 1] + 2 + (pre > 0 ? dp[pre - 1] : 0);
            }

            ans = Math.Max(ans, dp[i]);
        }

        return ans;
    }
}