//pass

namespace AdvancedTraining.Lesson30;

public class MergeSortedArray //leetcode_0088
{
    private static void Merge(int[] numbers1, int m, int[] numbers2, int n)
    {
        var index = numbers1.Length;
        while (m > 0 && n > 0)
            if (numbers1[m - 1] >= numbers2[n - 1])
                numbers1[--index] = numbers1[--m];
            else
                numbers1[--index] = numbers2[--n];
        while (m > 0) numbers1[--index] = numbers1[--m];
        while (n > 0) numbers1[--index] = numbers2[--n];
    }

    public static void Run()
    {
        int[] number1 = [1, 2, 3, 0, 0, 0];
        int[] number2 = [2, 5, 6];
        Merge(number1, 3, number2, 3);

        Console.WriteLine(string.Join(",", number1)); // 输出：[1,2,2,3,5,6]
    }
}