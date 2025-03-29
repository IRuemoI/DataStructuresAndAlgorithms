//pass

namespace AdvancedTraining.Lesson28;

public class FindFirstAndLastPositionOfElementInSortedArray //leetcode_0034
{
    private static int[] SearchRange(int[]? numbers, int target)
    {
        if (numbers == null || numbers.Length == 0) return [-1, -1];
        var l = LessMostRight(numbers, target) + 1;
        if (l == numbers.Length || numbers[l] != target) return [-1, -1];
        return new[] { l, LessMostRight(numbers, target + 1) };
    }

    private static int LessMostRight(int[] arr, int num)
    {
        var l = 0;
        var r = arr.Length - 1;
        var ans = -1;
        while (l <= r)
        {
            var m = l + ((r - l) >> 1);
            if (arr[m] < num)
            {
                ans = m;
                l = m + 1;
            }
            else
            {
                r = m - 1;
            }
        }

        return ans;
    }

    public static void Run()
    {
        foreach (var item in SearchRange([5, 7, 7, 8, 8, 10], 8)) Console.Write(item + ","); // 输出：[3,4]
    }
}