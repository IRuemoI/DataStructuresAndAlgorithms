#region

using Common.DataStructures.Heap;

#endregion

namespace AdvancedTraining.Lesson22;

// 本题测试链接 : https://leetcode.cn/problems/trapping-rain-water-ii/
public class TrappingRainWaterIi
{
    private static int TrapRainWater(int[,]? heightMap)
    {
        if (heightMap == null || heightMap.Length == 0 || heightMap[0, 0] == null ||
            heightMap.GetLength(1) == 0) return 0;

        var n = heightMap.GetLength(0);
        var m = heightMap.GetLength(1);
        var isEnter = new bool[n, m];
        var minHeap = new Heap<Node>((a, b) => a.Value - b.Value);
        for (var col = 0; col < m - 1; col++)
        {
            isEnter[0, col] = true;
            minHeap.Push(new Node(heightMap[0, col], 0, col));
        }

        for (var row = 0; row < n - 1; row++)
        {
            isEnter[row, m - 1] = true;
            minHeap.Push(new Node(heightMap[row, m - 1], row, m - 1));
        }

        for (var col = m - 1; col > 0; col--)
        {
            isEnter[n - 1, col] = true;
            minHeap.Push(new Node(heightMap[n - 1, col], n - 1, col));
        }

        for (var row = n - 1; row > 0; row--)
        {
            isEnter[row, 0] = true;
            minHeap.Push(new Node(heightMap[row, 0], row, 0));
        }

        var water = 0;
        var max = 0;
        while (!minHeap.IsEmpty())
        {
            var cur = minHeap.Pop();
            max = Math.Max(max, cur.Value);
            var r = cur.Row;
            var c = cur.Col;
            if (r > 0 && !isEnter[r - 1, c])
            {
                water += Math.Max(0, max - heightMap[r - 1, c]);
                isEnter[r - 1, c] = true;
                minHeap.Push(new Node(heightMap[r - 1, c], r - 1, c));
            }

            if (r < n - 1 && !isEnter[r + 1, c])
            {
                water += Math.Max(0, max - heightMap[r + 1, c]);
                isEnter[r + 1, c] = true;
                minHeap.Push(new Node(heightMap[r + 1, c], r + 1, c));
            }

            if (c > 0 && !isEnter[r, c - 1])
            {
                water += Math.Max(0, max - heightMap[r, c - 1]);
                isEnter[r, c - 1] = true;
                minHeap.Push(new Node(heightMap[r, c - 1], r, c - 1));
            }

            if (c < m - 1 && !isEnter[r, c + 1])
            {
                water += Math.Max(0, max - heightMap[r, c + 1]);
                isEnter[r, c + 1] = true;
                minHeap.Push(new Node(heightMap[r, c + 1], r, c + 1));
            }
        }

        return water;
    }

    public static void Run()
    {
        int[,] heightMap =
        {
            { 1, 4, 3, 1, 3, 2 },
            { 3, 2, 1, 3, 2, 4 },
            { 2, 3, 3, 2, 3, 1 }
        };
        Console.WriteLine(TrapRainWater(heightMap)); //输出4
    }

    private class Node(int v, int r, int c)
    {
        public readonly int Col = c;
        public readonly int Row = r;
        public readonly int Value = v;
    }
}