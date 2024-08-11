//测试通过

namespace Algorithms.Lesson40;

public class RotateMatrix
{
    private static void Rotate(int[][] matrix)
    {
        var a = 0;
        var b = 0;
        var c = matrix.Length - 1;
        var d = matrix[0].Length - 1;
        while (a < c) RotateEdge(matrix, a++, b++, c--, d--);
    }

    private static void RotateEdge(int[][] m, int a, int b, int c, int d)
    {
        var tmp = 0;
        for (var i = 0; i < d - b; i++)
        {
            tmp = m[a][b + i];
            m[a][b + i] = m[c - i][b];
            m[c - i][b] = m[c][d - i];
            m[c][d - i] = m[a + i][d];
            m[a + i][d] = tmp;
        }
    }

    private static void PrintMatrix(int[][] matrix)
    {
        for (var i = 0; i != matrix.Length; i++)
        {
            for (var j = 0; j != matrix[0].Length; j++) Console.Write(matrix[i][j] + " ");
            Console.WriteLine();
        }
    }

    public static void Run()
    {
        int[][] matrix =
        {
            new[] { 1, 2, 3, 4 },
            new[] { 5, 6, 7, 8 },
            new[] { 9, 10, 11, 12 },
            new[] { 13, 14, 15, 16 }
        };
        PrintMatrix(matrix);
        Rotate(matrix);
        Console.WriteLine("=========");
        PrintMatrix(matrix);
    }
}