//测试通过

namespace Algorithms.Lesson05;

// https://leetcode.cn/problems/count-of-range-sum/
public class CountOfRangeSum
{
    private static int CountRangeSum(int[]? numbers, int lower, int upper)
    {
        if (numbers == null || numbers.Length == 0) return 0;

        var sum = new long[numbers.Length];
        sum[0] = numbers[0];
        for (var i = 1; i < numbers.Length; i++) sum[i] = sum[i - 1] + numbers[i];

        return Process(sum, 0, sum.Length - 1, lower, upper);
    }

    private static int Process(long[] sum, int leftEdge, int rightEdge, int lower, int upper)
    {
        if (leftEdge == rightEdge) return sum[leftEdge] >= lower && sum[leftEdge] <= upper ? 1 : 0;

        var middle = leftEdge + ((rightEdge - leftEdge) >> 1);
        return Process(sum, leftEdge, middle, lower, upper) + Process(sum, middle + 1, rightEdge, lower, upper)
                                                            + Merge(sum, leftEdge, middle, rightEdge, lower, upper);
    }

    private static int Merge(long[] arr, int leftEdge, int middle, int rightEdge, int lower, int upper)
    {
        var ans = 0;
        var windowL = leftEdge;
        var windowR = leftEdge;
        // [windowL, windowR)
        for (var j = middle + 1; j <= rightEdge; j++)
        {
            var min = arr[j] - upper;
            var max = arr[j] - lower;
            while (windowR <= middle && arr[windowR] <= max) windowR++;

            while (windowL <= middle && arr[windowL] < min) windowL++;

            ans += windowR - windowL;
        }

        var help = new long[rightEdge - leftEdge + 1];
        var i = 0;
        var p1 = leftEdge;
        var p2 = middle + 1;
        while (p1 <= middle && p2 <= rightEdge) help[i++] = arr[p1] <= arr[p2] ? arr[p1++] : arr[p2++];

        while (p1 <= middle) help[i++] = arr[p1++];

        while (p2 <= rightEdge) help[i++] = arr[p2++];

        for (i = 0; i < help.Length; i++) arr[leftEdge + i] = help[i];

        return ans;
    }

    public static void Run()
    {
        int[] array = [-2, 5, -1];
        Console.WriteLine(CountRangeSum(array, -2, 2));
    }
}