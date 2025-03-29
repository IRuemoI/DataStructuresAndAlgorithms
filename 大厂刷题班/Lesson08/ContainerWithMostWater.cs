//pass

namespace AdvancedTraining.Lesson08;

// 本题测试链接 : https://leetcode.cn/problems/container-with-most-water/
public class ContainerWithMostWater
{
    private static int MaxArea1(int[] h)
    {
        var max = 0;
        var n = h.Length;
        for (var i = 0; i < n; i++)
            // h[i]
        for (var j = i + 1; j < n; j++)
            // h[j]
            max = Math.Max(max, Math.Min(h[i], h[j]) * (j - i));

        return max;
    }

    private static int MaxArea2(int[] h)
    {
        var max = 0;
        var l = 0;
        var r = h.Length - 1;
        while (l < r)
        {
            max = Math.Max(max, Math.Min(h[l], h[r]) * (r - l));
            if (h[l] > h[r])
                r--;
            else
                l++;
        }

        return max;
    }

    public static void Run()
    {
        Console.WriteLine(MaxArea1([1, 8, 6, 2, 5, 4, 8, 3, 7])); //输出49
        Console.WriteLine(MaxArea2([1, 8, 6, 2, 5, 4, 8, 3, 7])); //输出49
    }
}