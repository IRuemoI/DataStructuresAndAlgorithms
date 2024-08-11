//测试通过

namespace Algorithms.Lesson38;

public static class AppleMinBags
{
    public static int MinBags(int apple)
    {
        if (apple < 0) return -1;
        var bag8 = apple >> 3;
        var rest = apple - (bag8 << 3);
        while (bag8 >= 0)
            // rest 个
            if (rest % 6 == 0)
            {
                return bag8 + rest / 6;
            }
            else
            {
                bag8--;
                rest += 8;
            }

        return -1;
    }

    public static int MinBagAwesome(int apple)
    {
        if ((apple & 1) != 0)
            // 如果是奇数，返回-1
            return -1;
        if (apple < 18)
            return apple == 0 ? 0 : apple == 6 || apple == 8 ? 1 : apple == 12 || apple == 14 || apple == 16 ? 2 : -1;
        return (apple - 18) / 8 + 3;
    }
}

public class AppleMinBagsTest
{
    public static void Run()
    {
        Console.WriteLine("测试开始");
        for (var apple = 1; apple < 200; apple++)
        {
            var ans1 = AppleMinBags.MinBags(apple);
            var ans2 = AppleMinBags.MinBagAwesome(apple);

            if (ans1 != ans2)
            {
                Console.WriteLine("方法一:" + apple + " : " + ans1);
                Console.WriteLine("方法二:" + apple + " : " + ans2);
            }
        }

        Console.WriteLine("测试结束");
    }
}