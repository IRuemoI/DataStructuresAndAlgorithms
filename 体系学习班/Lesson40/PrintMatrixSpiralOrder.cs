//测试通过

namespace Algorithms.Lesson40;

public class PrintMatrixSpiralOrder
{
    // 顺时针打印二维数组
    private static void SpiralOrderPrint(int[][] matrix)
    {
        // 定义左上角和右下角的坐标
        var tR = 0;
        var tC = 0;
        var dR = matrix.Length - 1;
        var dC = matrix[0].Length - 1;
        // 当左上角坐标小于等于右下角坐标时，打印边框
        while (tR <= dR && tC <= dC) PrintEdge(matrix, tR++, tC++, dR--, dC--);
    }

    // 打印边框
    private static void PrintEdge(int[][] m, int tR, int tC, int dR, int dC)
    {
        // 如果左上角和右下角的行坐标相同，打印从左到右的列
        if (tR == dR)
        {
            for (var i = tC; i <= dC; i++) Console.Write(m[tR][i] + " ");
        }
        // 如果左上角和右下角的列坐标相同，打印从上到下的行
        else if (tC == dC)
        {
            for (var i = tR; i <= dR; i++) Console.Write(m[i][tC] + " ");
        }
        // 否则，打印从左到右、从上到下、从右到左、从下到上的边框
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
        [
            [1, 2, 3, 4],
            [5, 6, 7, 8],
            [9, 10, 11, 12],
            [13, 14, 15, 16]
        ];
        SpiralOrderPrint(matrix);
    }
}