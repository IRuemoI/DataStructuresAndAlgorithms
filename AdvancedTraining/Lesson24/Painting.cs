//pass
namespace AdvancedTraining.Lesson24;

public class Painting
{
    // N * M的棋盘
    // 每种颜色的格子数必须相同的
    // 相邻格子染的颜色必须不同
    // 所有格子必须染色
    // 返回至少多少种颜色可以完成任务

    private static int MinColors(int n, int m)
    {
        // 颜色数量是i
        for (var i = 2; i < n * m; i++)
        {
            var matrix = new int[n, m];
            // 下面这一句可知，需要的最少颜色数i，一定是N*M的某个因子
            if (n * m % i == 0 && Can(matrix, n, m, i)) return i;
        }

        return n * m;
    }

    // 在matrix上染色，返回只用pNum种颜色是否可以做到要求
    private static bool Can(int[,] matrix, int n, int m, int pNum)
    {
        var all = n * m;
        var every = all / pNum;
        var rest = new List<int> { 0 };
        for (var i = 1; i <= pNum; i++) rest.Add(every);
        return Process(matrix, n, m, pNum, 0, 0, rest);
    }

    private static bool Process(int[,] matrix, int n, int m, int pNum, int row, int col, IList<int> rest)
    {
        if (row == n) return true;
        if (col == m) return Process(matrix, n, m, pNum, row + 1, 0, rest);
        var left = col == 0 ? 0 : matrix[row, col - 1];
        var up = row == 0 ? 0 : matrix[row - 1, col];
        for (var color = 1; color <= pNum; color++)
            if (color != left && color != up && rest[color] > 0)
            {
                var count = rest[color];
                rest[color] = count - 1;
                matrix[row, col] = color;
                if (Process(matrix, n, m, pNum, row, col + 1, rest)) return true;
                rest[color] = count;
                matrix[row, col] = 0;
            }

        return false;
    }

    public static void Run()
    {
        // 根据代码16行的提示，打印出答案，看看是答案是哪个因子
        for (var n = 2; n < 10; n++)
        for (var m = 2; m < 10; m++)
        {
            Console.WriteLine("N   = " + n);
            Console.WriteLine("M   = " + m);
            Console.WriteLine("ans = " + MinColors(n, m));
            Console.WriteLine("===========");
        }
        // 打印答案，分析可知，是N*M最小的质数因子，原因不明，也不重要
        // 反正打表法猜出来了
    }
}