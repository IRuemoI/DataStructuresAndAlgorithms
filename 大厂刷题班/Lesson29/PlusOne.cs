//pass
namespace AdvancedTraining.Lesson29;

public class PlusOne //leetcode_0066
{
    private static int[] PlusOneCode(int[] digits)
    {
        var n = digits.Length;
        for (var i = n - 1; i >= 0; i--)
        {
            if (digits[i] < 9)
            {
                digits[i]++;
                return digits;
            }

            digits[i] = 0;
        }

        var ans = new int[n + 1];
        ans[0] = 1;
        return ans;
    }

    public static void Run()
    {
        foreach (var item in PlusOneCode([4, 3, 2, 1])) Console.Write(item + ","); //[4,3,2,2]
    }
}