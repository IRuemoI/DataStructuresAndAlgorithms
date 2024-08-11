//测试通过

namespace Algorithms.Lesson40;

public class PrintMatrixSpiralOrder
{
    private static void SpiralOrderPrint(int[][] matrix)
    {
        var tR = 0;
        var tC = 0;
        var dR = matrix.Length - 1;
        var dC = matrix[0].Length - 1;
        while (tR <= dR && tC <= dC) PrintEdge(matrix, tR++, tC++, dR--, dC--);
    }

    private static void PrintEdge(int[][] m, int tR, int tC, int dR, int dC)
    {
        if (tR == dR)
        {
            for (var i = tC; i <= dC; i++) Console.Write(m[tR][i] + " ");
        }
        else if (tC == dC)
        {
            for (var i = tR; i <= dR; i++) Console.Write(m[i][tC] + " ");
        }
        else
        {
            var curC = tC;
            var curR = tR;
            while (curC != dC)
            {
                Console.Write(m[tR][curC] + " ");
                curC++;
            }

            while (curR != dR)
            {
                Console.Write(m[curR][dC] + " ");
                curR++;
            }

            while (curC != tC)
            {
                Console.Write(m[dR][curC] + " ");
                curC--;
            }

            while (curR != tR)
            {
                Console.Write(m[curR][tC] + " ");
                curR--;
            }
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
        SpiralOrderPrint(matrix);
    }
}