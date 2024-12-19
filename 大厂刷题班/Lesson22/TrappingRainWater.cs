//pass
namespace AdvancedTraining.Lesson22;

// 本题测试链接 : https://leetcode.cn/problems/trapping-rain-water/
public class TrappingRainWater
{
    private static int Trap(int[]? arr)
    {
        if (arr == null || arr.Length < 2) return 0;
        var n = arr.Length;
        var l = 1;
        var leftMax = arr[0];
        var r = n - 2;
        var rightMax = arr[n - 1];
        var water = 0;
        while (l <= r)
            if (leftMax <= rightMax)
            {
                water += Math.Max(0, leftMax - arr[l]);
                leftMax = Math.Max(leftMax, arr[l++]);
            }
            else
            {
                water += Math.Max(0, rightMax - arr[r]);
                rightMax = Math.Max(rightMax, arr[r--]);
            }

        return water;
    }

    public static void Run()
    {
        Console.WriteLine(Trap([0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1])); //输出6
    }
}