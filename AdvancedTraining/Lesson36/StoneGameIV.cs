namespace AdvancedTraining.Lesson36;

// 来自哈喽单车
// 本题是leetcode原题 : https://leetcode.cn/problems/stone-game-iv/
public class StoneGameIv
{
    // 当前的！先手，会不会赢
    // 打表，不能发现规律
    private static bool WinnerSquareGame1(int n)
    {
        if (n == 0) return false;
        // 当前的先手，会尝试所有的情况，1，4，9，16，25，36....
        for (var i = 1; i * i <= n; i++)
            // 当前的先手，决定拿走 i * i 这个平方数
            // 它的对手会不会赢？ WinnerSquareGame1(n - i * i)
            if (!WinnerSquareGame1(n - i * i))
                return true;
        return false;
    }

    private static bool WinnerSquareGame2(int n)
    {
        var dp = new int[n + 1];
        dp[0] = -1;
        return Process2(n, dp);
    }

    private static bool Process2(int n, int[] dp)
    {
        if (dp[n] != 0) return dp[n] == 1;
        var ans = false;
        for (var i = 1; i * i <= n; i++)
            if (!Process2(n - i * i, dp))
            {
                ans = true;
                break;
            }

        dp[n] = ans ? 1 : -1;
        return ans;
    }

    private static bool WinnerSquareGame3(int n)
    {
        var dp = new bool[n + 1];
        for (var i = 1; i <= n; i++)
        for (var j = 1; j * j <= i; j++)
            if (!dp[i - j * j])
            {
                dp[i] = true;
                break;
            }

        return dp[n];
    }

    public static void Run()
    {
        Console.WriteLine(WinnerSquareGame1(7)); //输出false
        Console.WriteLine(WinnerSquareGame2(7)); //输出false
        Console.WriteLine(WinnerSquareGame3(7)); //输出false
    }
}