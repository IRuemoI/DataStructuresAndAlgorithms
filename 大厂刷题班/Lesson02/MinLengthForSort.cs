//pass

namespace AdvancedTraining.Lesson02;

// 本题测试链接 : https://leetcode.cn/problems/shortest-unsorted-continuous-subarray/description/
public class MinLengthForSort
{
    private static int FindUnsortedSubArray(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;
        var n = arr.Length;
        var right = -1;
        var max = int.MinValue;
        for (var i = 0; i < n; i++)
        {
            if (max > arr[i]) right = i;
            max = Math.Max(max, arr[i]);
        }

        var min = int.MaxValue;
        var left = n;
        for (var i = n - 1; i >= 0; i--)
        {
            if (min < arr[i]) left = i;
            min = Math.Min(min, arr[i]);
        }

        return Math.Max(0, right - left + 1);
    }


    public static void Run()
    {
        Console.WriteLine(FindUnsortedSubArray([2, 6, 4, 8, 10, 9, 15])); //输出5
    }
}