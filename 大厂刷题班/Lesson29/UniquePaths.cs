//pass
namespace AdvancedTraining.Lesson29;

public class UniquePaths //leetcode_0062
{
    // m 行
    // n 列
    // 下：m-1
    // 右：n-1
    private static int UniquePathsCode(int m, int n)
    {
        var right = n - 1;
        var all = m + n - 2;
        long o1 = 1;
        long o2 = 1;
        // o1乘进去的个数 一定等于 o2乘进去的个数
        for (int i = right + 1, j = 1; i <= all; i++, j++)
        {
            o1 *= i;
            o2 *= j;
            var gcd = Gcd(o1, o2);
            o1 /= gcd;
            o2 /= gcd;
        }

        return (int)o1;
    }

    // 调用的时候，请保证初次调用时，m和n都不为0
    private static long Gcd(long m, long n)
    {
        return n == 0 ? m : Gcd(n, m % n);
    }

    public static void Run()
    {
        Console.WriteLine(UniquePathsCode(3, 7)); //输出28
    }
}