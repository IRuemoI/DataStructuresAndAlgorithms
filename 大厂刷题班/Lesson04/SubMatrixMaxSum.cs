namespace AdvancedTraining.Lesson04;

public class SubMatrixMaxSum
{
    private static int MaxSum(int[][]? m)
    {
        if (m == null || m.Length == 0 || m[0].Length == 0) return 0;
        // O(N^2 * M)
        var row = m.GetLength(0);
        var column = m.GetLength(1);
        var max = int.MinValue;
        for (var i = 0; i < row; i++)
        {
            // i~j
            var s = new int[column];
            for (var j = i; j < row; j++)
            {
                for (var k = 0; k < column; k++) s[k] += m[j][k];
                max = Math.Max(max, MaxSubArray(s));
            }
        }

        return max;
    }

    private static int MaxSubArray(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var max = int.MinValue;
        var cur = 0;
        foreach (var item in arr)
        {
            cur += item;
            max = Math.Max(max, cur);
            cur = cur < 0 ? 0 : cur;
        }

        return max;
    }

    // 本题测试链接 : https://leetcode-cn.com/problems/max-submatrix-lcci/
    private static int[] GetMaxSubMatrix(int[][] m)
    {
        var row = m.GetLength(0);
        var column = m.GetLength(1);
        var max = int.MinValue;
        var a = 0;
        var b = 0;
        var c = 0;
        var d = 0;
        for (var i = 0; i < row; i++)
        {
            var s = new int[column];
            for (var j = i; j < row; j++)
            {
                var cur = 0;
                var begin = 0;
                for (var k = 0; k < column; k++)
                {
                    s[k] += m[j][k];
                    cur += s[k];
                    if (max < cur)
                    {
                        max = cur;
                        a = i;
                        b = begin;
                        c = j;
                        d = k;
                    }

                    if (cur < 0)
                    {
                        cur = 0;
                        begin = k + 1;
                    }
                }
            }
        }

        return [a, b, c, d];
    }
}