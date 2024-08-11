//通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson21;

public class MinPathSum
{
    private static int MinPathSum1(int[,]? m)
    {
        if (m == null || m.GetLength(0) == 0 || m[0, 0] == 0) return 0;

        var row = m.GetLength(0);
        var col = m.GetLength(1);
        var dp = new int[row, col];
        dp[0, 0] = m[0, 0];
        for (var i = 1; i < row; i++) dp[i, 0] = dp[i - 1, 0] + m[i, 0];

        for (var j = 1; j < col; j++) dp[0, j] = dp[0, j - 1] + m[0, j];

        for (var i = 1; i < row; i++)
        for (var j = 1; j < col; j++)
            dp[i, j] = Math.Min(dp[i - 1, j], dp[i, j - 1]) + m[i, j];

        return dp[row - 1, col - 1];
    }

    private static int MinPathSum2(int[,]? m)
    {
        if (m == null || m.GetLength(0) == 0 || m[0, 0] == 0) return 0;

        var row = m.GetLength(0);
        var col = m.GetLength(1);
        var dp = new int[col];
        dp[0] = m[0, 0];
        for (var j = 1; j < col; j++) dp[j] = dp[j - 1] + m[0, j];

        for (var i = 1; i < row; i++)
        {
            dp[0] += m[i, 0];
            for (var j = 1; j < col; j++) dp[j] = Math.Min(dp[j - 1], dp[j]) + m[i, j];
        }

        return dp[col - 1];
    }


    //���ڲ���
    private static int[,]? GenerateRandomMatrix(int rowSize, int colSize)
    {
        if (rowSize < 0 || colSize < 0) return null;

        var result = new int[rowSize, colSize];
        for (var i = 0; i != result.GetLength(0); i++)
        for (var j = 0; j != result.GetLength(1); j++)
            result[i, j] = (int)(Utility.GetRandomDouble * 100);

        return result;
    }

    //���ڲ���
    private static void PrintMatrix(int[,] matrix)
    {
        for (var i = 0; i != matrix.Length; i++)
        {
            for (var j = 0; j != matrix.GetLength(0); j++) Console.Write(matrix[i, j] + " ");

            Console.WriteLine();
        }
    }

    public static void Run()
    {
        var rowSize = 10;
        var colSize = 10;
        var m = GenerateRandomMatrix(rowSize, colSize);
        Console.WriteLine(MinPathSum1(m));
        Console.WriteLine(MinPathSum2(m));
    }
}