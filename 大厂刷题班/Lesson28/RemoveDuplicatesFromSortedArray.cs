//pass
namespace AdvancedTraining.Lesson28;

public class RemoveDuplicatesFromSortedArray //leetcode_0026
{
    private static int RemoveDuplicates(int[]? numbers)
    {
        if (numbers == null) return 0;
        if (numbers.Length < 2) return numbers.Length;
        var done = 0;
        for (var i = 1; i < numbers.Length; i++)
            if (numbers[i] != numbers[done])
                numbers[++done] = numbers[i];
        return done + 1;
    }

    public static void Run()
    {
        Console.WriteLine(RemoveDuplicates([0, 0, 1, 1, 1, 2, 2, 3, 3, 4])); //输出5
    }
}