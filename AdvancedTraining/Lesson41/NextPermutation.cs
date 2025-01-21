namespace AdvancedTraining.Lesson41;

//https://leetcode.cn/problems/next-permutation/description/
public class NextPermutation //leetcode_0031
{
    private static void NextPermutationCode(int[] numbers)
    {
        var n = numbers.Length;
        // 从右往左第一次降序的位置
        var firstLess = -1;
        for (var i = n - 2; i >= 0; i--)
            if (numbers[i] < numbers[i + 1])
            {
                firstLess = i;
                break;
            }

        if (firstLess < 0)
        {
            Reverse(numbers, 0, n - 1);
        }
        else
        {
            var rightClosestMore = -1;
            // 找最靠右的、同时比nums[firstLess]大的数，位置在哪
            // 这里其实也可以用二分优化，但是这种优化无关紧要了
            for (var i = n - 1; i > firstLess; i--)
                if (numbers[i] > numbers[firstLess])
                {
                    rightClosestMore = i;
                    break;
                }

            Swap(numbers, firstLess, rightClosestMore);
            Reverse(numbers, firstLess + 1, n - 1);
        }
    }

    private static void Reverse(int[] numbers, int l, int r)
    {
        while (l < r) Swap(numbers, l++, r--);
    }

    private static void Swap(int[] numbers, int i, int j)
    {
        (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
    }

    public static void Run()
    {
        int[] arr = [1, 1, 5];
        NextPermutationCode(arr);
        Console.WriteLine(string.Join(",", arr));
    }
}