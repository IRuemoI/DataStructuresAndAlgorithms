//测试通过

namespace Algorithms.Lesson38;

public static class MSumToN
{
    public static bool IsMSum1(int num)
    {
        for (var start = 1; start <= num; start++)
        {
            var sum = start;
            for (var j = start + 1; j <= num; j++)
            {
                if (sum + j > num) break;
                if (sum + j == num) return true;
                sum += j;
            }
        }

        return false;
    }

    public static bool IsMSum2(int num)
    {
        //return num == (num & (~num + 1));
        //return num == (num & (-num));
        return (num & (num - 1)) != 0;
    }
}

public class MSumToNTest
{
    public static void Run()
    {
        Console.WriteLine("测试开始");
        for (var num = 1; num < 5000; num++)
            if (MSumToN.IsMSum1(num) != MSumToN.IsMSum2(num))
                Console.WriteLine("出错啦！");
        Console.WriteLine("测试结束");
    }
}