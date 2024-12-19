//测试通过

namespace Algorithms.Lesson20;

public class HorseJump
{
    // 当前来到的位置是（x,y）
    // 还剩下rest步需要跳
    // 跳完rest步，正好跳到a，b的方法数是多少？
    // 10 * 9
    private static int Jump(int a, int b, int k)
    {
        return Process(0, 0, k, a, b);
    }

    private static int Process(int x, int y, int rest, int a, int b)
    {
        if (x < 0 || x > 9 || y < 0 || y > 8) return 0;

        if (rest == 0) return x == a && y == b ? 1 : 0;

        var ways = Process(x + 2, y + 1, rest - 1, a, b);
        ways += Process(x + 1, y + 2, rest - 1, a, b);
        ways += Process(x - 1, y + 2, rest - 1, a, b);
        ways += Process(x - 2, y + 1, rest - 1, a, b);
        ways += Process(x - 2, y - 1, rest - 1, a, b);
        ways += Process(x - 1, y - 2, rest - 1, a, b);
        ways += Process(x + 1, y - 2, rest - 1, a, b);
        ways += Process(x + 2, y - 1, rest - 1, a, b);
        return ways;
    }

    private static int Dp(int a, int b, int k)
    {
        var dp = new int[10, 9, k + 1];
        dp[a, b, 0] = 1;
        for (var rest = 1; rest <= k; rest++)
        for (var x = 0; x < 10; x++)
        for (var y = 0; y < 9; y++)
        {
            var ways = Pick(dp, x + 2, y + 1, rest - 1);
            ways += Pick(dp, x + 1, y + 2, rest - 1);
            ways += Pick(dp, x - 1, y + 2, rest - 1);
            ways += Pick(dp, x - 2, y + 1, rest - 1);
            ways += Pick(dp, x - 2, y - 1, rest - 1);
            ways += Pick(dp, x - 1, y - 2, rest - 1);
            ways += Pick(dp, x + 1, y - 2, rest - 1);
            ways += Pick(dp, x + 2, y - 1, rest - 1);
            dp[x, y, rest] = ways;
        }

        return dp[0, 0, k];
    }

    //防止下标越界的方法
    private static int Pick(int[,,] dp, int x, int y, int rest)
    {
        if (x < 0 || x > 9 || y < 0 || y > 8) return 0;

        return dp[x, y, rest];
    }

    private static int Ways(int a, int b, int step)
    {
        return F(0, 0, step, a, b);
    }

    private static int F(int i, int j, int step, int a, int b)
    {
        if (i < 0 || i > 9 || j < 0 || j > 8) return 0;

        if (step == 0) return i == a && j == b ? 1 : 0;

        return F(i - 2, j + 1, step - 1, a, b) + F(i - 1, j + 2, step - 1, a, b) + F(i + 1, j + 2, step - 1, a, b)
               + F(i + 2, j + 1, step - 1, a, b) + F(i + 2, j - 1, step - 1, a, b) + F(i + 1, j - 2, step - 1, a, b)
               + F(i - 1, j - 2, step - 1, a, b) + F(i - 2, j - 1, step - 1, a, b);
    }

    private static int WaysDp(int a, int b, int s)
    {
        var dp = new int[10, 9, s + 1];
        dp[a, b, 0] = 1;
        for (var step = 1; step <= s; step++)
            // 按层来
        for (var i = 0; i < 10; i++)
        for (var j = 0; j < 9; j++)
            dp[i, j, step] = GetValue(dp, i - 2, j + 1, step - 1) +
                             GetValue(dp, i - 1, j + 2, step - 1) +
                             GetValue(dp, i + 1, j + 2, step - 1) +
                             GetValue(dp, i + 2, j + 1, step - 1) +
                             GetValue(dp, i + 2, j - 1, step - 1) +
                             GetValue(dp, i + 1, j - 2, step - 1) +
                             GetValue(dp, i - 1, j - 2, step - 1) +
                             GetValue(dp, i - 2, j - 1, step - 1);

        return dp[0, 0, s];
    }

    // 在dp表中，得到dp[i,j,step]的值，但如果(i，j)位置越界的话，返回0；
    private static int GetValue(int[,,] dp, int i, int j, int step)
    {
        if (i < 0 || i > 9 || j < 0 || j > 8) return 0;

        return dp[i, j, step];
    }

    public static void Run()
    {
        var x = 7;
        var y = 7;
        var step = 10;
        Console.WriteLine(Ways(x, y, step));
        Console.WriteLine(Dp(x, y, step));
        Console.WriteLine(Jump(x, y, step));
        Console.WriteLine(WaysDp(x, y, step));
    }
}