namespace AdvancedTraining.Lesson50;

public class ErectTheFence //Problem_0587
{
    private static int[][] OuterTrees(int[][] points)
    {
        // 栈用于存储结果  
        var stack = new List<int[]>();

        // 将点按照x坐标排序（如果x相同则按y排序）  
        var sortedPoints = points.OrderBy(p => p[0]).ThenBy(p => p[1]).ToArray();

        // 第一次遍历，从左到右添加点到栈中  
        foreach (var item in sortedPoints)
        {
            while (stack.Count >= 2 && Cross(stack[^2], stack[^1], item) <= 0) stack.RemoveAt(stack.Count - 1);
            stack.Add(item);
        }

        // 第二次遍历，从右到左添加点到栈中（但不包括已经添加过的点）  
        for (var i = sortedPoints.Length - 2; i >= 0; i--)
            if (!stack.Contains(sortedPoints[i]))
            {
                while (stack.Count >= 2 && Cross(stack[^2], stack[^1], sortedPoints[i]) <= 0)
                    stack.RemoveAt(stack.Count - 1);
                stack.Add(sortedPoints[i]);
            }

        // 移除重复的顶点  
        stack = stack.Distinct().ToList();

        return stack.ToArray();
    }

    // 叉乘的实现  
    private static int Cross(int[] a, int[] b, int[] c)
    {
        return (b[1] - a[1]) * (c[0] - b[0]) - (b[0] - a[0]) * (c[1] - b[1]);
    }

    public static void Run()
    {
        // 示例用法  
        int[][] points =
        [
            [0, 0], [2, 2], [1, 0], [0, 4], [3, 3], [2, 4]
        ];
        var outerTrees = OuterTrees(points);

        // 输出结果  
        foreach (var tree in outerTrees) Console.WriteLine($"({tree[0]}, {tree[1]})");
    }
}