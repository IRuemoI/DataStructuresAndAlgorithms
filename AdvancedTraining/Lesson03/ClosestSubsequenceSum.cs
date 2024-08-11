namespace AdvancedTraining.Lesson03;

// 本题测试链接 : https://leetcode.cn/problems/closest-subsequence-sum/
// 本题数据量描述:
// 1 <= nums.length <= 40
// -10^7 <= nums[i] <= 10^7
// -10^9 <= goal <= 10^9
// 通过这个数据量描述可知，需要用到分治，因为数组长度不大
// 而值很大，用动态规划的话，表会爆
public class ClosestSubsequenceSum
{
    private static readonly int[] L = new int[1 << 20];
    private static readonly int[] R = new int[1 << 20];

    private static int MinAbsDifference(int[]? arr, int goal)
    {
        if (arr == null || arr.Length == 0) return goal;
        var le = Process(arr, 0, arr.Length >> 1, 0, 0, L);
        var re = Process(arr, arr.Length >> 1, arr.Length, 0, 0, R);
        Array.Sort(L, 0, le);
        Array.Sort(R, 0, re--);
        var ans = Math.Abs(goal);
        for (var i = 0; i < le; i++)
        {
            var rest = goal - L[i];
            while (re > 0 && Math.Abs(rest - R[re - 1]) <= Math.Abs(rest - R[re])) re--;
            ans = Math.Min(ans, Math.Abs(rest - R[re]));
        }

        return ans;
    }

    private static int Process(int[] numbers, int index, int end, int sum, int fill, int[] arr)
    {
        if (index == end)
        {
            arr[fill++] = sum;
        }
        else
        {
            fill = Process(numbers, index + 1, end, sum, fill, arr);
            fill = Process(numbers, index + 1, end, sum + numbers[index], fill, arr);
        }

        return fill;
    }


    public static void Run()
    {
        Console.WriteLine(MinAbsDifference([7, -9, 15, -2], -5)); //输出1
    }
}