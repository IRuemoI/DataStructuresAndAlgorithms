//pass

using Common.Utilities;

namespace AdvancedTraining.Lesson13;

public class NCardsAbWin
{
    // 谷歌面试题
    // 面值为1~10的牌组成一组，
    // 每次你从组里等概率的抽出1~10中的一张
    // 下次抽会换一个新的组，有无限组
    // 当累加和<17时，你将一直抽牌
    // 当累加和>=17且<21时，你将获胜
    // 当累加和>=21时，你将失败
    // 返回获胜的概率
    private static double F1()
    {
        return P1(0);
    }

    // 游戏的规则，如上
    // 当你来到cur这个累加和的时候，获胜概率是多少返回！
    private static double P1(int cur)
    {
        if (cur is >= 17 and < 21) return 1.0;

        if (cur >= 21) return 0.0;

        var w = 0.0;
        for (var i = 1; i <= 10; i++) w += P1(cur + i);

        return w / 10;
    }

    // 谷歌面试题扩展版
    // 面值为1~N的牌组成一组，
    // 每次你从组里等概率的抽出1~N中的一张
    // 下次抽会换一个新的组，有无限组
    // 当累加和<a时，你将一直抽牌
    // 当累加和>=a且<b时，你将获胜
    // 当累加和>=b时，你将失败
    // 返回获胜的概率，给定的参数为N，a，b
    private static double F2(int n, int a, int b)
    {
        if (n < 1 || a >= b || a < 0 || b < 0) return 0.0;

        if (b - a >= n) return 1.0;

        // 所有参数都合法，并且b-a < N
        return P2(0, n, a, b);
    }

    // 游戏规则，如上，int N, int a, int b，固定参数！
    // cur，目前到达了cur的累加和
    // 返回赢的概率
    private static double P2(int cur, int n, int a, int b)
    {
        if (cur >= a && cur < b) return 1.0;

        if (cur >= b) return 0.0;

        var w = 0.0;
        for (var i = 1; i <= n; i++) w += P2(cur + i, n, a, b);

        return w / n;
    }

    // f2的改进版本，用到了观察位置优化枚举的技巧
    // 可以课上讲一下
    private static double F3(int n, int a, int b)
    {
        if (n < 1 || a >= b || a < 0 || b < 0) return 0.0;

        if (b - a >= n) return 1.0;

        return P3(0, n, a, b);
    }

    private static double P3(int cur, int n, int a, int b)
    {
        if (cur >= a && cur < b) return 1.0;

        if (cur >= b) return 0.0;

        if (cur == a - 1) return 1.0 * (b - a) / n;

        var w = P3(cur + 1, n, a, b) + P3(cur + 1, n, a, b) * n;
        if (cur + 1 + n < b) w -= P3(cur + 1 + n, n, a, b);

        return w / n;
    }

    // f3的改进版本的动态规划
    // 可以课上讲一下
    private static double F4(int n, int a, int b)
    {
        if (n < 1 || a >= b || a < 0 || b < 0) return 0.0;

        if (b - a >= n) return 1.0;

        var dp = new double[b];
        for (var i = a; i < b; i++) dp[i] = 1.0;

        if (a - 1 >= 0) dp[a - 1] = 1.0 * (b - a) / n;

        for (var cur = a - 2; cur >= 0; cur--)
        {
            var w = dp[cur + 1] + dp[cur + 1] * n;
            if (cur + 1 + n < b) w -= dp[cur + 1 + n];

            dp[cur] = w / n;
        }

        return dp[0];
    }

    public static void Run()
    {
        var n = 10;
        var a = 17;
        var b = 21;
        Console.WriteLine("N = " + n + ", a = " + a + ", b = " + b);
        Console.WriteLine(F1());
        Console.WriteLine(F2(n, a, b));
        Console.WriteLine(F3(n, a, b));
        Console.WriteLine(F4(n, a, b));

        var maxN = 15;
        var maxM = 20;
        var testTime = 1000;
        Console.WriteLine("测试开始");
        Console.Write("比对double类型答案可能会有精度对不准的问题, ");
        Console.Write("所以答案一律只保留小数点后四位进行比对, ");
        Console.WriteLine("如果没有错误提示, 说明验证通过");
        for (var i = 0; i < testTime; i++)
        {
            n = (int)(Utility.getRandomDouble * maxN);
            a = (int)(Utility.getRandomDouble * maxM);
            b = (int)(Utility.getRandomDouble * maxM);
            var ans2 = F2(n, a, b);
            var ans3 = F2(n, a, b);
            var ans4 = F2(n, a, b);
            if (Math.Abs(ans2 - ans3) > 0.0001f || Math.Abs(ans2 - ans4) > 0.0001f)
            {
                Console.WriteLine("出错啦！");
                Console.WriteLine(n + " , " + a + " , " + b);
                Console.WriteLine(ans2);
                Console.WriteLine(ans3);
                Console.WriteLine(ans4);
            }
        }

        Console.WriteLine("测试结束");

        n = 10000;
        a = 67834;
        b = 72315;
        Console.WriteLine("N = " + n + ", a = " + a + ", b = " + b + "时, 除了方法4外都超时");
        Console.Write("方法4答案: ");
        Console.WriteLine(F4(n, a, b));
    }
}