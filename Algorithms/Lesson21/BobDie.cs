//测试通过

namespace Algorithms.Lesson21;

public class BobDie
{
    private static double LivePossibility1(int row, int col, int k, int n, int m)
    {
        return Process(row, col, k, n, m) / Math.Pow(4, k);
    }

    // 目前在row，col位置，还有rest步要走，走完了如果还在棋盘中就获得1个生存点，返回总的生存点数
    private static long Process(int row, int col, int rest, int n, int m)
    {
        if (row < 0 || row == n || col < 0 || col == m) return 0;

        // 还在棋盘中！
        if (rest == 0) return 1;

        // 还在棋盘中！还有步数要走
        var up = Process(row - 1, col, rest - 1, n, m);
        var down = Process(row + 1, col, rest - 1, n, m);
        var left = Process(row, col - 1, rest - 1, n, m);
        var right = Process(row, col + 1, rest - 1, n, m);
        return up + down + left + right;
    }

    private static double LivePossibility2(int row, int col, int k, int n, int m)
    {
        var dp = new long[n, m, k + 1];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
            dp[i, j, 0] = 1;

        for (var rest = 1; rest <= k; rest++)
        for (var r = 0; r < n; r++)
        for (var c = 0; c < m; c++)
        {
            dp[r, c, rest] = Pick(dp, n, m, r - 1, c, rest - 1);
            dp[r, c, rest] += Pick(dp, n, m, r + 1, c, rest - 1);
            dp[r, c, rest] += Pick(dp, n, m, r, c - 1, rest - 1);
            dp[r, c, rest] += Pick(dp, n, m, r, c + 1, rest - 1);
        }

        return dp[row, col, k] / Math.Pow(4, k);
    }

    private static long Pick(long[,,] dp, int n, int m, int r, int c, int rest)
    {
        if (r < 0 || r == n || c < 0 || c == m) return 0;

        return dp[r, c, rest];
    }

    public static void Run()
    {
        Console.WriteLine(LivePossibility1(6, 6, 10, 50, 50));
        Console.WriteLine(LivePossibility2(6, 6, 10, 50, 50));
    }
}