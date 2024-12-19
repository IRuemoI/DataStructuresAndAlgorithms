//测试通过

namespace Algorithms.Lesson25;

// 测试链接：https://leetcode.cn/problems/largest-rectangle-in-histogram
public static class LargestRectangleInHistogram
{
    public static int LargestRectangleArea1(int[]? height)
    {
        if (height == null || height.Length == 0) return 0;

        var maxArea = 0;
        var stack = new Stack<int>();
        for (var i = 0; i < height.Length; i++)
        {
            while (stack.Count != 0 && height[i] <= height[stack.Peek()])
            {
                var j = stack.Pop();
                var k = stack.Count == 0 ? -1 : stack.Peek();
                var curArea = (i - k - 1) * height[j];
                maxArea = Math.Max(maxArea, curArea);
            }

            stack.Push(i);
        }

        while (stack.Count != 0)
        {
            var j = stack.Pop();
            var k = stack.Count == 0 ? -1 : stack.Peek();
            var curArea = (height.Length - k - 1) * height[j];
            maxArea = Math.Max(maxArea, curArea);
        }

        return maxArea;
    }

    public static int LargestRectangleArea2(int[]? height)
    {
        if (height == null || height.Length == 0) return 0;

        var n = height.Length;
        var stack = new int[n];
        var si = -1;
        var maxArea = 0;
        for (var i = 0; i < height.Length; i++)
        {
            while (si != -1 && height[i] <= height[stack[si]])
            {
                var j = stack[si--];
                var k = si == -1 ? -1 : stack[si];
                var curArea = (i - k - 1) * height[j];
                maxArea = Math.Max(maxArea, curArea);
            }

            stack[++si] = i;
        }

        while (si != -1)
        {
            var j = stack[si--];
            var k = si == -1 ? -1 : stack[si];
            var curArea = (height.Length - k - 1) * height[j];
            maxArea = Math.Max(maxArea, curArea);
        }

        return maxArea;
    }
}

public class LargestRectangleInHistogramTest
{
    public static void Run()
    {
        int[] heights1 = [2, 1, 5, 6, 2, 3];
        int[] heights2 = [2, 4];
        Console.WriteLine(LargestRectangleInHistogram.LargestRectangleArea1(heights1)); //10
        Console.WriteLine(LargestRectangleInHistogram.LargestRectangleArea2(heights2)); //4
    }
}