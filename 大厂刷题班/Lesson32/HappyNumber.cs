//pass

namespace AdvancedTraining.Lesson32;

public class HappyNumber //leetcode_0202
{
    private static bool IsHappy1(int n)
    {
        var set = new HashSet<int>();
        while (n != 1)
        {
            var sum = 0;
            while (n != 0)
            {
                var r = n % 10;
                sum += r * r;
                n /= 10;
            }

            n = sum;
            if (!set.Add(n)) break;
        }

        return n == 1;
    }

    // 实验代码
    private static SortedSet<int> sum(int n)
    {
        var set = new SortedSet<int>();
        while (!set.Contains(n))
        {
            set.Add(n);
            var sum = 0;
            while (n != 0)
            {
                sum += n % 10 * (n % 10);
                n /= 10;
            }

            n = sum;
        }

        return set;
    }

    private static bool IsHappy2(int n)
    {
        while (n != 1 && n != 4)
        {
            var sum = 0;
            while (n != 0)
            {
                sum += n % 10 * (n % 10);
                n /= 10;
            }

            n = sum;
        }

        return n == 1;
    }

    public static void Run()
    {
        for (var i = 1; i < 100; i++) Console.WriteLine(sum(i));

        Console.WriteLine(IsHappy1(19));
        Console.WriteLine(IsHappy2(19));
    }
}