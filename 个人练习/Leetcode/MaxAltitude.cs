namespace CustomTraining.Leetcode;

public class MaxAltitude
{
    // 定义一个方法，用于计算给定高度数组中，限制范围内最大高度
    private static int[] MaxAltitudeCode(int[] heights, int limit)
    {
        // 如果高度数组为空或限制为0，则返回空数组
        if (heights.Length == 0 || limit == 0) return [];

        // 定义一个双向链表，用于存储高度
        var deque = new LinkedList<int>();
        // 定义一个结果数组，用于存储最大高度
        var result = new int[heights.Length - limit + 1];
        // 定义一个索引变量
        var i = 0;
        // 遍历高度数组
        for (i = 0; i < limit; i++)
        {
            // 如果双向链表不为空且链表最后一个元素小于当前高度，则移除链表最后一个元素
            while (deque.Count != 0 && deque.Last?.Value < heights[i]) deque.RemoveLast();

            // 将当前高度添加到双向链表
            deque.AddLast(heights[i]);
        }

        // 如果双向链表为空，则返回结果数组
        if (deque.First == null) return result;

        // 将双向链表第一个元素赋值给结果数组的第一个元素
        result[0] = deque.First.Value;
        // 继续遍历高度数组
        for (i = limit; i < heights.Length; i++)
        {
            // 如果双向链表第一个元素等于当前高度减去限制，则移除双向链表第一个元素
            if (deque.First.Value == heights[i - limit]) deque.RemoveFirst();

            // 如果双向链表不为空且链表最后一个元素小于当前高度，则移除链表最后一个元素
            while (deque.Count != 0 && deque.Last?.Value < heights[i]) deque.RemoveLast();

            // 将当前高度添加到双向链表
            deque.AddLast(heights[i]);
            // 将双向链表第一个元素赋值给结果数组的下一个元素
            result[i - limit + 1] = deque.First.Value;
        }


        // 返回结果数组
        return result;
    }


    public static void Run()
    {
        int[] heights = [14, 2, 27, -5, 28, 13, 39];
        var limit = 3;
        Console.WriteLine(string.Join(", ", MaxAltitudeCode(heights, limit)));
    }
}