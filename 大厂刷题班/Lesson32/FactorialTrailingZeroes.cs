//pass

namespace AdvancedTraining.Lesson32;

public class FactorialTrailingZeroes //leetcode_0172
{
    private static int TrailingZeroes(int n)
    {
        var ans = 0;
        while (n != 0)
        {
            n /= 5;
            ans += n;
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(TrailingZeroes(5)); //输出1
    }
}