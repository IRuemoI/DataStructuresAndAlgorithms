#region

using Common.DataStructures.Heap;

#endregion

namespace AdvancedTraining.Lesson35;

//https://leetcode.cn/problems/top-k-frequent-elements/description/
public class TopKFrequentElements //Problem_0347
{
    private static int[] TopKFrequent(int[] numbers, int k)
    {
        var map = new Dictionary<int, Node>();
        foreach (var num in numbers)
            if (!map.TryGetValue(num, out var value))
                map[num] = new Node(num);
            else
                value.Count++;

        var heap = new Heap<Node>((x, y) => x.Count - y.Count);
        foreach (var node in map.Values)
        {
            if (heap.Count < k || (heap.Count == k && node.Count > heap.Peek().Count)) heap.Push(node);

            if (heap.Count > k) heap.Pop();
        }

        var ans = new int[k];
        var index = 0;
        while (!heap.IsEmpty) ans[index++] = heap.Pop().Num;

        return ans;
    }

    public static void Run()
    {
        Console.WriteLine(string.Join(", ", TopKFrequent([1, 1, 1, 2, 2, 3], 2))); //输出: [1,2]
    }

    private class Node(int k)
    {
        public readonly int Num = k;
        public int Count = 1;
    }
}