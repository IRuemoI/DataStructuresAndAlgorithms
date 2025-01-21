namespace AdvancedTraining.Lesson37;

public class MaximalSquare //leetcode_0221
{
    private static int MaximalSquareCode(char[,]? m)
    {
        if (m == null || m.Length == 0 || m.GetLength(1) == 0) return 0;
        var row = m.GetLength(0);
        var col = m.GetLength(1);
        var dp = new int[row + 1, col + 1];
        var max = 0;
        for (var i = 0; i < row; i++)
            if (m[i, 0] == '1')
            {
                dp[i, 0] = 1;
                max = 1;
            }

        for (var j = 1; j < col; j++)
            if (m[0, j] == '1')
            {
                dp[0, j] = 1;
                max = 1;
            }

        for (var i = 1; i < row; i++)
        for (var j = 1; j < col; j++)
            if (m[i, j] == '1')
            {
                dp[i, j] = Math.Min(Math.Min(dp[i - 1, j], dp[i, j - 1]), dp[i - 1, j - 1]) + 1;
                max = Math.Max(max, dp[i, j]);
            }

        return max * max;
    }

    public static void Run()
    {
        char[,] matrix =
        {
            { '1', '0', '1', '0', '0' },
            { '1', '0', '1', '1', '1' },
            { '1', '1', '1', '1', '1' },
            { '1', '0', '0', '1', '0' }
        };
        Console.WriteLine(MaximalSquareCode(matrix)); //输出：4
    }
}