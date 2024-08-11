//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson23;

public class NQueens
{
    private static int Num1(int n)
    {
        if (n < 1) return 0;

        var record = new int[n];
        return Process1(0, record, n);
    }

    // 当前来到i行，一共是0~N-1行
    // 在i行上放皇后，所有列都尝试
    // 必须要保证跟之前所有的皇后不打架
    // int[] record record[x] = y 之前的第x行的皇后，放在了y列上
    // 返回：不关心i以上发生了什么，i.... 后续有多少合法的方法数
    private static int Process1(int i, int[] record, int n)
    {
        if (i == n) return 1;

        var res = 0;
        // i行的皇后，放哪一列呢？j列，
        for (var j = 0; j < n; j++)
            if (IsValid(record, i, j))
            {
                record[i] = j;
                res += Process1(i + 1, record, n);
            }

        return res;
    }

    private static bool IsValid(int[] record, int i, int j)
    {
        // 0..i-1
        for (var k = 0; k < i; k++)
            if (j == record[k] || Math.Abs(record[k] - j) == Math.Abs(i - k))
                return false;

        return true;
    }

    // 请不要超过32皇后问题
    private static int Num2(int n)
    {
        if (n < 1 || n > 32) return 0;

        // 如果你是13皇后问题，limit 最右13个1，其他都是0
        var limit = n == 32 ? -1 : (1 << n) - 1;
        return Process2(limit, 0, 0, 0);
    }

    // 7皇后问题
    // limit : 0....0 1 1 1 1 1 1 1
    // 之前皇后的列影响：colLim
    // 之前皇后的左下对角线影响：leftDiaLim
    // 之前皇后的右下对角线影响：rightDiaLim
    private static int Process2(int limit, int colLim, int leftDiaLim, int rightDiaLim)
    {
        if (colLim == limit) return 1;

        // pos中所有是1的位置，是你可以去尝试皇后的位置
        var pos = limit & ~(colLim | leftDiaLim | rightDiaLim);
        var res = 0;
        while (pos != 0)
        {
            var mostRightOne = pos & (~pos + 1);
            pos = pos - mostRightOne;
            res += Process2(limit, colLim | mostRightOne, (leftDiaLim | mostRightOne) << 1,
                (rightDiaLim | mostRightOne) >>> 1);
        }

        return res;
    }

    public static void Run()
    {
        var n = 15;
        Utility.RestartStopwatch();
        Console.WriteLine(Num2(n));
        Console.WriteLine("cost time: " + Utility.GetStopwatchElapsedMilliseconds() + "ms");
        Utility.RestartStopwatch();
        Console.WriteLine(Num1(n));
        Console.WriteLine("cost time: " + Utility.GetStopwatchElapsedMilliseconds() + "ms");
    }
}