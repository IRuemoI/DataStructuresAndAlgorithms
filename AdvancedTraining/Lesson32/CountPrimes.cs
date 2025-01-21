//pass
namespace AdvancedTraining.Lesson32;
public class CountPrimes //leetcode_0204
{
    private static int CountPrimesCode(int n)
    {
        if (n < 3) return 0;
        // j已经不是素数了，f[j] = true;
        var f = new bool[n];
        var count = n / 2; // 所有偶数都不要，还剩几个数
        // 跳过了1、2    3、5、7、
        for (var i = 3; i * i < n; i += 2)
        {
            if (f[i]) continue;
            // 3 -> 3 * 3 = 9   3 * 5 = 15   3 * 7 = 21
            // 7 -> 7 * 7 = 49  7 * 9 = 63
            // 13 -> 13 * 13  13 * 15
            for (var j = i * i; j < n; j += 2 * i)
                if (!f[j])
                {
                    --count;
                    f[j] = true;
                }
        }

        return count;
    }

    public static void Run()
    {
        Console.WriteLine(CountPrimesCode(10)); //输出4
    }
}