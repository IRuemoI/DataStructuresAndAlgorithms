namespace AdvancedTraining.Lesson01;

public class LongestIncreasingPath
{
    private static int LongestIncreasingPath1(int[][] matrix)
    {
        var ans = 0;
        var n = matrix.Length;
        var m = matrix[0].Length;
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
            ans = Math.Max(ans, Process1(matrix, i, j));
        return ans;
    }

    // 从m[i][j]开始走，走出来的最长递增链，返回！
    private static int Process1(int[][] m, int i, int j)
    {
        var up = i > 0 && m[i][j] < m[i - 1][j] ? Process1(m, i - 1, j) : 0;
        var down = i < m.Length - 1 && m[i][j] < m[i + 1][j] ? Process1(m, i + 1, j) : 0;
        var left = j > 0 && m[i][j] < m[i][j - 1] ? Process1(m, i, j - 1) : 0;
        var right = j < m[0].Length - 1 && m[i][j] < m[i][j + 1] ? Process1(m, i, j + 1) : 0;
        return Math.Max(Math.Max(up, down), Math.Max(left, right)) + 1;
    }

    private static int LongestIncreasingPath2(int[][] matrix)
    {
        var ans = 0;
        var n = matrix.Length;
        var m = matrix[0].Length;
        var dp = new int[n, m];
        for (var i = 0; i < n; i++)
        for (var j = 0; j < m; j++)
            ans = Math.Max(ans, Process2(matrix, i, j, dp));
        return ans;
    }

    // 从m[i][j]开始走，走出来的最长递增链，返回！
    private static int Process2(int[][] m, int i, int j, int[,] dp)
    {
        if (dp[i, j] != 0) return dp[i, j];
        // (i,j)不越界
        var up = i > 0 && m[i][j] < m[i - 1][j] ? Process2(m, i - 1, j, dp) : 0;
        var down = i < m.Length - 1 && m[i][j] < m[i + 1][j] ? Process2(m, i + 1, j, dp) : 0;
        var left = j > 0 && m[i][j] < m[i][j - 1] ? Process2(m, i, j - 1, dp) : 0;
        var right = j < m[0].Length - 1 && m[i][j] < m[i][j + 1] ? Process2(m, i, j + 1, dp) : 0;
        var ans = Math.Max(Math.Max(up, down), Math.Max(left, right)) + 1;
        dp[i, j] = ans;
        return ans;
    }

    public static void Run()
    {
        int[][] arr =
        [
            [9, 9, 4],
            [6, 6, 8],
            [2, 1, 1]
        ];
        Console.WriteLine("参数一:");
        Console.WriteLine($"方法1:{LongestIncreasingPath1(arr)}"); //输出4
        Console.WriteLine($"方法2:{LongestIncreasingPath2(arr)}"); //输出4

        arr =
        [
            [1]
        ];
        Console.WriteLine("参数二:");
        Console.WriteLine($"方法1:{LongestIncreasingPath1(arr)}"); //输出1
        Console.WriteLine($"方法2:{LongestIncreasingPath2(arr)}"); //输出1
    }
}