//pass

namespace AdvancedTraining.Lesson04;

// 本题测试链接 : https://leetcode.cn/problems/maximum-subarray/
public class SubArrayMaxSum
{
    private static int MaxSubArray(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        var max = int.MinValue;
        var cur = 0;
        foreach (var item in arr)
        {
            cur += item;
            max = Math.Max(max, cur);
            cur = cur < 0 ? 0 : cur;
        }

        return max;
    }

    private static int MaxSubArray2(int[]? arr)
    {
        if (arr == null || arr.Length == 0) return 0;
        // 上一步，dp的值
        // dp[0]
        var pre = arr[0];
        var max = arr[0];
        for (var i = 1; i < arr.Length; i++)
        {
            pre = Math.Max(arr[i], arr[i] + pre);
            max = Math.Max(max, pre);
        }

        return max;
    }

    public static void Run()
    {
        Console.WriteLine(MaxSubArray([-2, 1, -3, 4, -1, 2, 1, -5, 4])); //输出6
        Console.WriteLine(MaxSubArray2([-2, 1, -3, 4, -1, 2, 1, -5, 4])); //输出6
    }
}