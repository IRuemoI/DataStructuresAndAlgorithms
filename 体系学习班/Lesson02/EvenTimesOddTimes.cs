//测试通过

namespace Algorithms.Lesson02;

public class EvenTimesOddTimes //偶次奇次
{
    // arr中，只有一种数，出现奇数次
    private static void PrintOddTimesNum1(int[] arr)
    {
        var eor = 0;
        foreach (var t in arr) eor ^= t;

        Console.WriteLine(eor);
    }

    // arr中，有两种数，出现奇数次
    //leetcode:https://leetcode.cn/problems/shu-zu-zhong-shu-zi-chu-xian-de-ci-shu-lcof/
    private static void PrintOddTimesNum2(int[] arr)
    {
        //使用两种不同的维度来筛选这两个数字
        //第一个维度：哪两个数字出现了奇数次
        var sumEor = 0;
        foreach (var item in arr) sumEor ^= item; //根据异或的特性,最终sumEor是两个所求奇数异或
        //第二个维度：二进制形式上最后一位'1'位置不同的两类
        var rightmost1Bit = sumEor & -sumEor; //因为异或是不同为1，所以出现1的地方两个数的二进制位是不同的

        var number1 = 0;
        foreach (var item in arr)
            if ((item & rightmost1Bit) != 0) //将最右侧1相同的异或到一起
                number1 ^= item; //因为出现偶数次的都被消掉，循环后的结果就是两个奇数中的一个

        var number2 = sumEor ^ number1;

        Console.WriteLine($"number1:{number1}, number2:{number2}");
    }

    //统计一个无符号整型中1的个数
    private static int Bit1Counts(uint n)
    {
        var count = 0;

        //   011011010000
        //   000000010000     1

        //   011011000000

        while (n != 0)
        {
            var rightOne = n & (~n + 1);
            count++;
            n ^= rightOne;
            // N -= rightOne
        }

        return count;
    }

    public static void Run()
    {
        int[] arr1 = [3, 3, 2, 3, 1, 1, 1, 3, 1, 1, 1];
        PrintOddTimesNum1(arr1);

        int[] arr2 = [4, 3, 4, 2, 2, 2, 4, 1, 1, 1, 3, 3, 1, 1, 1, 4, 2, 2];
        PrintOddTimesNum2(arr2);
        Console.WriteLine("----------");
        int[] arr3 = [1, 2, 4, 1, 4, 3, 12, 3];
        PrintOddTimesNum2(arr3);

        Console.WriteLine(Bit1Counts(0b1101));
    }
}