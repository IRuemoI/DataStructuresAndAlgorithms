//测试通过

namespace Algorithms.Lesson38;

public static class EatGrass
{
    // 如果n份草，最终先手赢，返回"先手"
    // 如果n份草，最终后手赢，返回"后手"
    public static string WhoWin(int n)
    {
        if (n < 5) return n == 0 || n == 2 ? "后手" : "先手";
        // 进到这个过程里来，当前的先手，先选
        var want = 1;
        while (want <= n)
        {
            if (WhoWin(n - want).Equals("后手")) return "先手";
            if (want <= n / 4)
                want *= 4;
            else
                break;
        }

        return "后手";
    }

    public static string Winner1(int n)
    {
        if (n < 5) return n == 0 || n == 2 ? "后手" : "先手";
        var @base = 1;
        while (@base <= n)
        {
            if (Winner1(n - @base).Equals("后手")) return "先手";
            if (@base > n / 4)
                // 防止base*4之后溢出
                break;
            @base *= 4;
        }

        return "后手";
    }

    public static string Winner2(int n)
    {
        if (n % 5 == 0 || n % 5 == 2)
            return "后手";
        return "先手";
    }
}

public class EatGrassTest
{
    public static void Run()
    {
        Console.WriteLine("测试开始");
        for (var i = 0; i <= 50; i++)
        {
            var ans1 = EatGrass.WhoWin(i);
            var ans2 = EatGrass.Winner1(i);
            var ans3 = EatGrass.Winner2(i);
            if (ans1 != ans2 || ans2 != ans3) Console.WriteLine($"ans1:{ans1},ans2:{ans2},ans3:{ans3}");
        }

        Console.WriteLine("测试结束");
    }
}