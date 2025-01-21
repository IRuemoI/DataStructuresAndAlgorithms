//pass
namespace AdvancedTraining.Lesson32;

public class MaximumProductSubArray //leetcode_0152
{
    private static double Max(double[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0; // 报错！
        var n = arr.Length;
        // 上一步的最大
        var preMax = arr[0];
        // 上一步的最小
        var preMin = arr[0];
        var ans = arr[0];
        for (var i = 1; i < n; i++)
        {
            var p1 = arr[i];
            var p2 = arr[i] * preMax;
            var p3 = arr[i] * preMin;
            var curMax = Math.Max(Math.Max(p1, p2), p3);
            var curMin = Math.Min(Math.Min(p1, p2), p3);
            ans = Math.Max(ans, curMax);
            preMax = curMax;
            preMin = curMin;
        }

        return ans;
    }


    private static int MaxProduct(int[] numbers)
    {
        var ans = numbers[0];
        var min = numbers[0];
        var max = numbers[0];
        for (var i = 1; i < numbers.Length; i++)
        {
            var curMin = Math.Min(numbers[i], Math.Min(min * numbers[i], max * numbers[i]));
            var curMax = Math.Max(numbers[i], Math.Max(min * numbers[i], max * numbers[i]));
            min = curMin;
            max = curMax;
            ans = Math.Max(ans, max);
        }

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(MaxProduct([2, 3, -2, 4])); //输出6
    }
}