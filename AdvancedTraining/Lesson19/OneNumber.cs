#region

using Common.Utilities;

#endregion

namespace AdvancedTraining.Lesson19;

public class OneNumber
{
    private static int Solution1(int num)
    {
        if (num < 1) return 0;
        var count = 0;
        for (var i = 1; i != num + 1; i++) count += Get1Numbers(i);
        return count;
    }

    private static int Get1Numbers(int num)
    {
        var res = 0;
        while (num != 0)
        {
            if (num % 10 == 1) res++;
            num /= 10;
        }

        return res;
    }


    // 1 ~ num 这个范围上，画了几道1
    private static int Solution2(int num)
    {
        if (num < 1) return 0;
        // num -> 13625
        // len = 5位数
        var len = GetLenOfNum(num);
        if (len == 1) return 1;
        // num  13625
        // tmp1 10000
        // 
        // num  7872328738273
        // tmp1 1000000000000
        var tmp1 = PowerBaseOf10(len - 1);
        // num最高位 num / tmp1
        var first = num / tmp1;
        // 最高1 N % tmp1 + 1
        // 最高位first tmp1
        var firstOneNum = first == 1 ? num % tmp1 + 1 : tmp1;
        // 除去最高位之外，剩下1的数量
        // 最高位1 10(k-2次方) * (k-1) * 1
        // 最高位first 10(k-2次方) * (k-1) * first
        var otherOneNum = first * (len - 1) * (tmp1 / 10);
        return firstOneNum + otherOneNum + Solution2(num % tmp1);
    }

    private static int GetLenOfNum(int num)
    {
        var len = 0;
        while (num != 0)
        {
            len++;
            num /= 10;
        }

        return len;
    }

    private static int PowerBaseOf10(int @base)
    {
        return (int)Math.Pow(10, @base);
    }

    public static void Run()
    {
        var num = 50000000;

        Utility.RestartStopwatch();
        Console.WriteLine(Solution1(num));
        Console.WriteLine("cost time: " + Utility.GetStopwatchElapsedMilliseconds() + " ms");

        Utility.RestartStopwatch();
        Console.WriteLine(Solution2(num));
        Console.WriteLine("cost time: " + Utility.GetStopwatchElapsedMilliseconds() + " ms");
    }
}