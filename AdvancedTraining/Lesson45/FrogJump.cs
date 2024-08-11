namespace AdvancedTraining.Lesson45;

//https://leetcode.cn/problems/frog-jump/description/
public class FrogJump //Problem_0403
{
    private static bool CanCross(int[] stones)
    {
        var set = new HashSet<int>();
        foreach (var num in stones) set.Add(num);
        var dp = new Dictionary<int, Dictionary<int, bool>>();
        return Jump(1, 1, stones[^1], set, dp);
    }

    private static bool Jump(int cur, int pre, int end, HashSet<int> set, Dictionary<int, Dictionary<int, bool>> dp)
    {
        if (cur == end) return true;
        if (!set.Contains(cur)) return false;
        if (dp.ContainsKey(cur) && dp[cur].ContainsKey(pre)) return dp[cur][pre];
        var ans = (pre > 1 && Jump(cur + pre - 1, pre - 1, end, set, dp)) || Jump(cur + pre, pre, end, set, dp) ||
                  Jump(cur + pre + 1, pre + 1, end, set, dp);
        if (!dp.ContainsKey(cur)) dp[cur] = new Dictionary<int, bool>();
        if (!dp[cur].ContainsKey(pre)) dp[cur][pre] = ans;
        return ans;
    }


    public static void Run()
    {
        Console.WriteLine(CanCross([0, 1, 3, 5, 6, 8, 12, 17])); //输出True
    }
}