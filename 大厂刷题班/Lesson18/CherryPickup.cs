namespace AdvancedTraining.Lesson18;
// 牛客的测试链接：
// https://www.nowcoder.com/questionTerminal/8ecfe02124674e908b2aae65aad4efdf
// 把如下的全部代码拷贝进java编辑器
// 把文件大类名字改成Main，可以直接通过

public class CherryPickup
{
    public static void Run()
    {
        var matrix = new[,]
        {
            { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1 }
        };
        var ans = Code(matrix);
        Console.WriteLine(ans);
    }

    private static int Code(int[,] grid)
    {
        var n = grid.GetLength(0);
        var m = grid.GetLength(1);
        var dp = new int[n, m, n];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
        for (var k = 0; k < n; k++)
            dp[i, j, k] = int.MinValue;
        var ans = Process(grid, 0, 0, 0, dp);
        return ans < 0 ? 0 : ans;
    }

    private static int Process(int[,] grid, int x1, int y1, int x2, int[,,] dp)
    {
        if (x1 == grid.GetLength(0) || y1 == grid.GetLength(1) || x2 == grid.GetLength(0) || x1 + y1 - x2 >= grid.GetLength(1))
            return int.MinValue;
        if (dp[x1, y1, x2] != int.MinValue) return dp[x1, y1, x2];
        if (x1 == grid.GetLength(0) - 1 && y1 == grid.GetLength(1) - 1)
        {
            dp[x1, y1, x2] = grid[x1, y1];
            return dp[x1, y1, x2];
        }

        var next = int.MinValue;
        next = Math.Max(next, Process(grid, x1 + 1, y1, x2 + 1, dp));
        next = Math.Max(next, Process(grid, x1 + 1, y1, x2, dp));
        next = Math.Max(next, Process(grid, x1, y1 + 1, x2, dp));
        next = Math.Max(next, Process(grid, x1, y1 + 1, x2 + 1, dp));
        if (grid[x1, y1] == -1 || grid[x2, x1 + y1 - x2] == -1 || next == -1)
        {
            dp[x1, y1, x2] = -1;
            return dp[x1, y1, x2];
        }

        if (x1 == x2)
        {
            dp[x1, y1, x2] = grid[x1, y1] + next;
            return dp[x1, y1, x2];
        }

        dp[x1, y1, x2] = grid[x1, y1] + grid[x2, x1 + y1 - x2] + next;
        return dp[x1, y1, x2];
    }
}