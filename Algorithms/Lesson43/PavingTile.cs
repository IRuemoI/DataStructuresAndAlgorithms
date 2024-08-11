namespace Algorithms.Lesson43;

public class PavingTile
{
    /*
     * 2*M铺地的问题非常简单，这个是解决N*M铺地的问题
     */

    private static int Ways1(int n, int m)
    {
        if (n < 1 || m < 1 || ((n * m) & 1) != 0) return 0;
        if (n == 1 || m == 1) return 1;
        var pre = new int[m]; // pre代表-1行的状况
        for (var i = 0; i < pre.Length; i++) pre[i] = 1;
        return Process(pre, 0, n);
    }

    // pre 表示level-1行的状态
    // level表示，正在level行做决定
    // N 表示一共有多少行 固定的
    // level-2行及其之上所有行，都摆满砖了
    // level做决定，让所有区域都满，方法数返回
    private static int Process(int[] pre, int level, int n)
    {
        if (level == n)
        {
            // base case
            foreach (var element in pre)
                if (element == 0)
                    return 0;

            return 1;
        }

        // 没到终止行，可以选择在当前的level行摆瓷砖
        var op = GetOp(pre);
        return Dfs(op, 0, level, n);
    }

    // op[i] == 0 可以考虑摆砖
    // op[i] == 1 只能竖着向上
    private static int Dfs(int[] op, int col, int level, int n)
    {
        // 在列上自由发挥，玩深度优先遍历，当col来到终止列，i行的决定做完了
        // 轮到i+1行，做决定
        if (col == op.Length) return Process(op, level + 1, n);
        var ans = 0;
        // col位置不横摆
        ans += Dfs(op, col + 1, level, n); // col位置上不摆横转
        // col位置横摆, 向右
        if (col + 1 < op.Length && op[col] == 0 && op[col + 1] == 0)
        {
            op[col] = 1;
            op[col + 1] = 1;
            ans += Dfs(op, col + 2, level, n);
            op[col] = 0;
            op[col + 1] = 0;
        }

        return ans;
    }

    private static int[] GetOp(int[] pre)
    {
        var cur = new int[pre.Length];
        for (var i = 0; i < pre.Length; i++) cur[i] = pre[i] ^ 1;
        return cur;
    }

    // Min (N,M) 不超过 32
    private static int Ways2(int n, int m)
    {
        if (n < 1 || m < 1 || ((n * m) & 1) != 0) return 0;
        if (n == 1 || m == 1) return 1;
        var max = Math.Max(n, m);
        var min = Math.Min(n, m);
        var pre = (1 << min) - 1;
        return Process2(pre, 0, max, min);
    }

    // 上一行的状态，是pre，limit是用来对齐的，固定参数不用管
    // 当前来到i行，一共N行，返回填满的方法数
    private static int Process2(int pre, int i, int n, int m)
    {
        if (i == n)
            // base case
            return pre == (1 << m) - 1 ? 1 : 0;
        var op = ~pre & ((1 << m) - 1);
        return Dfs2(op, m - 1, i, n, m);
    }

    private static int Dfs2(int op, int col, int level, int n, int m)
    {
        if (col == -1) return Process2(op, level + 1, n, m);
        var ans = 0;
        ans += Dfs2(op, col - 1, level, n, m);
        if ((op & (1 << col)) == 0 && col - 1 >= 0 && (op & (1 << (col - 1))) == 0)
            ans += Dfs2(op | (3 << (col - 1)), col - 2, level, n, m);
        return ans;
    }

    // 记忆化搜索的解
    // Min(N,M) 不超过 32
    private static int Ways3(int n, int m)
    {
        if (n < 1 || m < 1 || ((n * m) & 1) != 0) return 0;
        if (n == 1 || m == 1) return 1;
        var max = Math.Max(n, m);
        var min = Math.Min(n, m);
        var pre = (1 << min) - 1;
        var dp = new int[1 << min, max + 1];
        for (var i = 0; i < dp.GetLength(0); i++)
        for (var j = 0; j < dp.GetLength(1); j++)
            dp[i, j] = -1;
        return Process3(pre, 0, max, min, dp);
    }

    private static int Process3(int pre, int i, int n, int m, int[,] dp)
    {
        if (dp[pre, i] != -1) return dp[pre, i];
        int ans;
        if (i == n)
        {
            ans = pre == (1 << m) - 1 ? 1 : 0;
        }
        else
        {
            var op = ~pre & ((1 << m) - 1);
            ans = Dfs3(op, m - 1, i, n, m, dp);
        }

        dp[pre, i] = ans;
        return ans;
    }

    private static int Dfs3(int op, int col, int level, int n, int m, int[,] dp)
    {
        if (col == -1) return Process3(op, level + 1, n, m, dp);
        var ans = 0;
        ans += Dfs3(op, col - 1, level, n, m, dp);
        if (col > 0 && (op & (3 << (col - 1))) == 0) ans += Dfs3(op | (3 << (col - 1)), col - 2, level, n, m, dp);
        return ans;
    }

    // 严格位置依赖的动态规划解
    private static int Ways4(int n, int m)
    {
        if (n < 1 || m < 1 || ((n * m) & 1) != 0) return 0;
        if (n == 1 || m == 1) return 1;
        var big = n > m ? n : m;
        var small = big == n ? m : n;
        var sn = 1 << small;
        var limit = sn - 1;
        var dp = new int[sn];
        dp[limit] = 1;
        var cur = new int[sn];
        for (var level = 0; level < big; level++)
        {
            for (var status = 0; status < sn; status++)
                if (dp[status] != 0)
                {
                    var op = ~status & limit;
                    Dfs4(dp[status], op, 0, small - 1, cur);
                }

            for (var i = 0; i < sn; i++) dp[i] = 0;
            (dp, cur) = (cur, dp);
        }

        return dp[limit];
    }

    private static void Dfs4(int way, int op, int index, int end, int[] cur)
    {
        if (index == end)
        {
            cur[op] += way;
        }
        else
        {
            Dfs4(way, op, index + 1, end, cur);
            if (((3 << index) & op) == 0)
                // 11 << index 可以放砖
                Dfs4(way, op | (3 << index), index + 1, end, cur);
        }
    }

    public static void Run()
    {
        var n = 8;
        var m = 6;
        Console.WriteLine(Ways1(n, m));
        Console.WriteLine(Ways2(n, m));
        Console.WriteLine(Ways3(n, m));
        Console.WriteLine(Ways4(n, m));

        n = 10;
        m = 10;
        Console.WriteLine("=========");
        Console.WriteLine(Ways3(n, m));
        Console.WriteLine(Ways4(n, m));
    }
}