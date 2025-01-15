namespace CustomTraining.Leetcode;

public static class MyPow
{
    private static double MyPowCode(double x, int n)
    {
        if (x == 0.0d) return 0.0d;

        long b = n;
        var res = 1.0d;
        if (b < 0)
        {
            x = 1 / x;
            b = -b;
        }

        //类似程序员升职记的40倍放大器:https://www.bilibili.com/video/BV1ab4y1S7ke/?p=11
        while (b > 0)
        {
            if ((b & 1) == 1) res *= x;

            x *= x;
            b >>= 1;
        }

        return res;
    }

    public static void Run()
    {
        Console.WriteLine(MyPowCode(2, 10));
        Console.WriteLine(MyPowCode(2, -3));
    }
}