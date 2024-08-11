namespace Algorithms.Lesson40;

public class ZigZagPrintMatrix
{
    private static void PrintMatrixZigZag(int[][] matrix)
    {
        var tR = 0;
        var tC = 0;
        var dR = 0;
        var dC = 0;
        var endR = matrix.Length - 1;
        var endC = matrix[0].Length - 1;
        var fromUp = false;
        while (tR != endR + 1)
        {
            PrintLevel(matrix, tR, tC, dR, dC, fromUp);
            tR = tC == endC ? tR + 1 : tR;
            tC = tC == endC ? tC : tC + 1;
            dC = dR == endR ? dC + 1 : dC;
            dR = dR == endR ? dR : dR + 1;
            fromUp = !fromUp;
        }

        Console.WriteLine();
    }

    private static void PrintLevel(int[][] m, int tR, int tC, int dR, int dC, bool f)
    {
        if (f)
            while (tR != dR + 1)
                Console.Write(m[tR++][tC--] + " ");
        else
            while (dR != tR - 1)
                Console.Write(m[dR--][dC++] + " ");
    }

    public static void Run()
    {
        int[][] matrix =
        {
            new[] { 1, 2, 3, 4 },
            new[] { 5, 6, 7, 8 },
            new[] { 9, 10, 11, 12 }
        };
        PrintMatrixZigZag(matrix);
    }
}