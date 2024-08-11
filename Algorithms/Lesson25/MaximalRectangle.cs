//测试通过

namespace Algorithms.Lesson25;

// 测试链接：https://leetcode.cn/problems/maximal-rectangle/
public static class MaximalRectangle
{
    public static int Code(char[,]? map)
    {
        if (map == null || map.GetLength(0) == 0 || map.GetLength(1) == 0) return 0;

        var maxArea = 0;
        var height = new int[map.GetLength(1)];
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++) height[j] = map[i, j] == '0' ? 0 : height[j] + 1;
            maxArea = Math.Max(MaxRecFromBottom(height), maxArea);
        }

        return maxArea;
    }

    // height是正方图数组
    private static int MaxRecFromBottom(int[]? height)
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
}

public class MaximalRectangleTest
{
    public static void Run()
    {
        char[,] matrix1 =
        {
            { '1', '0', '1', '0', '0' },
            { '1', '0', '1', '1', '1' },
            { '1', '1', '1', '1', '1' },
            { '1', '0', '0', '1', '0' }
        };
        char[,] matrix2 = { { '0' } };
        char[,] matrix3 = { { '1' } };

        Console.WriteLine(MaximalRectangle.Code(matrix1)); //6
        Console.WriteLine(MaximalRectangle.Code(matrix2)); //0
        Console.WriteLine(MaximalRectangle.Code(matrix3)); //1
    }
}