namespace AdvancedTraining.Lesson34;

//pass
public class FindTheDuplicateNumber //leetcode_0287
{
    private static int FindDuplicate(int[]? numbers)
    {
        if (numbers == null || numbers.Length < 2) return -1;
        var slow = numbers[0];
        var fast = numbers[numbers[0]];
        while (slow != fast)
        {
            slow = numbers[slow];
            fast = numbers[numbers[fast]];
        }

        fast = 0;
        while (slow != fast)
        {
            fast = numbers[fast];
            slow = numbers[slow];
        }

        return slow;
    }

    public static void Run()
    {
        Console.WriteLine(FindDuplicate([3, 1, 3, 4, 2])); //输出3
    }
}