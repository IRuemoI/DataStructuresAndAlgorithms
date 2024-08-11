namespace AdvancedTraining.Lesson28;

public class RomanToInteger //Problem_0013
{
    private static int RomanToInt(string s)
    {
        // C     M     X   C     I   X
        // 100  1000  10   100   1   10
        var nums = new int[s.Length];
        for (var i = 0; i < s.Length; i++)
            nums[i] = s[i] switch
            {
                'M' => 1000,
                'D' => 500,
                'C' => 100,
                'L' => 50,
                'X' => 10,
                'V' => 5,
                'I' => 1,
                _ => nums[i]
            };

        var sum = 0;
        for (var i = 0; i < nums.Length - 1; i++)
            if (nums[i] < nums[i + 1])
                sum -= nums[i];
            else
                sum += nums[i];
        return sum + nums[^1];
    }

    public static void Run()
    {
        Console.WriteLine(RomanToInt("MCMXCIV")); // 输出: 1994
    }
}