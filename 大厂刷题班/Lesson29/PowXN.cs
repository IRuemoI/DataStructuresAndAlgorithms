//pass
namespace AdvancedTraining.Lesson29;

public class PowXn //leetcode_0050
{
    private static int Pow(int a, int n)
    {
        var ans = 1;
        var t = a;
        while (n != 0)
        {
            if ((n & 1) != 0) ans *= t;
            t *= t;
            n >>= 1;
        }

        return ans;
    }

    // x的n次方，n可能是负数
    private static double MyPow(double x, int n)
    {
        if (n == 0) return 1D;
        var pow = Math.Abs(n == int.MinValue ? n + 1 : n);
        var t = x;
        var ans = 1D;
        while (pow != 0)
        {
            if ((pow & 1) != 0) ans *= t;
            pow >>= 1;
            t = t * t;
        }

        if (n == int.MinValue) ans *= x;
        return n < 0 ? 1D / ans : ans;
    }

    public static void Run()
    {
        Console.WriteLine(Pow(2, 10)); //输出：1024
        Console.WriteLine(MyPow(2.10000, 3)); //输出：9.26100
    }
}