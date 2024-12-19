#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson08;

//todo:待修复
public class SnakeGame
{
    private static int Walk1(int[,]? matrix)
    {
        if (matrix == null || matrix.Length == 0 || matrix.GetLength(1) == 0) return 0;
        var res = int.MinValue;
        for (var i = 0; i < matrix.Length; i++)
        for (var j = 0; j < matrix.GetLength(1); j++)
        {
            var ans = process(matrix, i, j);
            res = Math.Max(res, Math.Max(ans[0], ans[1]));
        }

        return res;
    }

    private static int Zuo(int[,]? matrix)
    {
        if (matrix == null || matrix.Length == 0 || matrix.GetLength(1) == 0) return 0;
        var ans = 0;
        for (var i = 0; i < matrix.Length; i++)
        for (var j = 0; j < matrix.GetLength(1); j++)
        {
            var cur = F(matrix, i, j);
            ans = Math.Max(ans, Math.Max(cur.No, cur.Yes));
        }

        return ans;
    }

    // 蛇从某一个最左列，且最优的空降点降落
    // 沿途走到(i,j)必须停！
    // 返回，一次能力也不用，获得的最大成长值
    // 返回，用了一次能力，获得的最大成长值
    // 如果蛇从某一个最左列，且最优的空降点降落，不用能力，怎么都到不了(i,j)，那么no = -1
    // 如果蛇从某一个最左列，且最优的空降点降落，用了一次能力，怎么都到不了(i,j)，那么yes = -1
    private static Info F(int[,] matrix, int i, int j)
    {
        if (j == 0)
        {
            // 最左列
            var no = Math.Max(matrix[i, 0], -1);
            var yes = Math.Max(-matrix[i, 0], -1);
            return new Info(no, yes);
        }

        // j > 0 不在最左列
        var preNo = -1;
        var preYes = -1;
        var pre = F(matrix, i, j - 1);
        preNo = Math.Max(pre.No, preNo);
        preYes = Math.Max(pre.Yes, preYes);
        if (i > 0)
        {
            pre = F(matrix, i - 1, j - 1);
            preNo = Math.Max(pre.No, preNo);
            preYes = Math.Max(pre.Yes, preYes);
        }

        if (i < matrix.Length - 1)
        {
            pre = F(matrix, i + 1, j - 1);
            preNo = Math.Max(pre.No, preNo);
            preYes = Math.Max(pre.Yes, preYes);
        }

        var no1 = preNo == -1 ? -1 : Math.Max(-1, preNo + matrix[i, j]);
        // 能力只有一次，是之前用的！
        var p1 = preYes == -1 ? -1 : Math.Max(-1, preYes + matrix[i, j]);
        // 能力只有一次，就当前用！
        var p2 = preNo == -1 ? -1 : Math.Max(-1, preNo - matrix[i, j]);
        var yes1 = Math.Max(Math.Max(p1, p2), -1);
        return new Info(no1, yes1);
    }

    // 从假想的最优左侧到达(i,j)的旅程中
    // 0) 在没有使用过能力的情况下，返回路径最大和，没有可能到达的话，返回负
    // 1) 在使用过能力的情况下，返回路径最大和，没有可能到达的话，返回负
    private static int[] process(int[,] m, int i, int j)
    {
        if (j == 0)
            // (i,j)就是最左侧的位置
            return new[] { m[i, j], -m[i, j] };
        var preAns = process(m, i, j - 1);
        // 所有的路中，完全不使用能力的情况下，能够到达的最好长度是多大
        var preUnuse = preAns[0];
        // 所有的路中，使用过一次能力的情况下，能够到达的最好长度是多大
        var preUse = preAns[1];
        if (i - 1 >= 0)
        {
            preAns = process(m, i - 1, j - 1);
            preUnuse = Math.Max(preUnuse, preAns[0]);
            preUse = Math.Max(preUse, preAns[1]);
        }

        if (i + 1 < m.Length)
        {
            preAns = process(m, i + 1, j - 1);
            preUnuse = Math.Max(preUnuse, preAns[0]);
            preUse = Math.Max(preUse, preAns[1]);
        }

        // preUnuse 之前旅程，没用过能力
        // preUse 之前旅程，已经使用过能力了
        var no = -1; // 之前没使用过能力，当前位置也不使用能力，的最优解
        var yes = -1; // 不管是之前使用能力，还是当前使用了能力，请保证能力只使用一次，最优解
        if (preUnuse >= 0)
        {
            no = m[i, j] + preUnuse;
            yes = -m[i, j] + preUnuse;
        }

        if (preUse >= 0) yes = Math.Max(yes, m[i, j] + preUse);
        return new[] { no, yes };
    }

    private static int Walk2(int[,]? matrix)
    {
        if (matrix == null || matrix.GetLength(0) == 0 || matrix.GetLength(1) == 0) return 0;
        var max = int.MinValue;
        var dp = new int[matrix.GetLength(0), matrix.GetLength(1), 2];
        for (var i = 0; i < dp.Length; i++)
        {
            dp[i, 0, 0] = matrix[i, 0];
            dp[i, 0, 1] = -matrix[i, 0];
            max = Math.Max(max, Math.Max(dp[i, 0, 0], dp[i, 0, 1]));
        }

        for (var j = 1; j < matrix.GetLength(1); j++)
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            var preUnuse = dp[i, j - 1, 0];
            var preUse = dp[i, j - 1, 1];
            if (i - 1 >= 0)
            {
                preUnuse = Math.Max(preUnuse, dp[i - 1, j - 1, 0]);
                preUse = Math.Max(preUse, dp[i - 1, j - 1, 1]);
            }

            if (i + 1 < matrix.Length)
            {
                preUnuse = Math.Max(preUnuse, dp[i + 1, j - 1, 0]);
                preUse = Math.Max(preUse, dp[i + 1, j - 1, 1]);
            }

            dp[i, j, 0] = -1;
            dp[i, j, 1] = -1;
            if (preUnuse >= 0)
            {
                dp[i, j, 0] = matrix[i, j] + preUnuse;
                dp[i, j, 1] = -matrix[i, j] + preUnuse;
            }

            if (preUse >= 0) dp[i, j, 1] = Math.Max(dp[i, j, 1], matrix[i, j] + preUse);
            max = Math.Max(max, Math.Max(dp[i, j, 0], dp[i, j, 1]));
        }

        return max;
    }

    private static int[,] GetRandomStringArray(int row, int col, int value)
    {
        var arr = new int[row, col];
        for (var i = 0; i < arr.GetLength(0); i++)
        for (var j = 0; j < arr.GetLength(1); j++)
            arr[i, j] = (int)(Utility.getRandomDouble * value) * (Utility.getRandomDouble > 0.5 ? -1 : 1);
        return arr;
    }

    public static void Run()
    {
        const int n = 7;
        const int m = 7;
        const int v = 10;
        var times = 1000;
        for (var i = 0; i < times; i++)
        {
            var r = (int)(Utility.getRandomDouble * (n + 1));
            var c = (int)(Utility.getRandomDouble * (m + 1));
            var matrix = GetRandomStringArray(r, c, v);
            var ans1 = Zuo(matrix);
            var ans2 = Walk2(matrix);
            if (ans1 != ans2)
            {
                for (var j = 0; j < matrix.GetLength(0); j++)
                for (var k = 0; k < matrix.GetLength(1); k++)
                    Console.WriteLine(matrix[j, k]);
                Console.WriteLine("出错了   ans1: " + ans1 + "   ans2:" + ans2);
                break;
            }
        }

        Console.WriteLine("测试结束");
    }

    private class Info(int no, int yes)
    {
        public readonly int No = no;
        public readonly int Yes = yes;
    }
}