namespace AdvancedTraining.Lesson32;

public class NumberOf1Bits //Problem_0191
{
    // n的二进制形式，有几个1？
    private static int HammingWeight1(int n)
    {
        var bits = 0;
        while (n != 0)
        {
            bits++;
            var rightOne = n & -n;
            n ^= rightOne;
        }

        return bits;
    }

    private static int HammingWeight2(int n)
    {
        n = (n & 0x55555555) + ((n >>> 1) & 0x55555555);
        n = (n & 0x33333333) + ((n >>> 2) & 0x33333333);
        n = (n & 0x0f0f0f0f) + ((n >>> 4) & 0x0f0f0f0f);
        n = (n & 0x00ff00ff) + ((n >>> 8) & 0x00ff00ff);
        n = (n & 0x0000ffff) + ((n >>> 16) & 0x0000ffff);
        return n;
    }

    public static void Run()
    {
        Console.WriteLine(HammingWeight1(2147483645)); //输出30
        Console.WriteLine(HammingWeight2(2147483645)); //输出30
    }
}