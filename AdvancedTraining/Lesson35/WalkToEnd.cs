#region

using Common.DataStructures.Heap;

#endregion

namespace AdvancedTraining.Lesson35;

// 来自网易
// map[i,j] == 0，代表(i,j)是海洋，渡过的话代价是2
// map[i,j] == 1，代表(i,j)是陆地，渡过的话代价是1
// map[i,j] == 2，代表(i,j)是障碍，无法渡过
// 每一步上、下、左、右都能走，返回从左上角走到右下角最小代价是多少，如果无法到达返回-1
public class WalkToEnd
{
    private static int MinCost(int[,] map)
    {
        if (map[0, 0] == 2) return -1;
        var n = map.GetLength(0);
        var m = map.GetLength(1);
        var minHeap = new Heap<Node>((a, b) => a.Cost - b.Cost);
        var visited = new bool[n, m];
        Add(map, 0, 0, 0, minHeap, visited);
        while (!minHeap.IsEmpty)
        {
            var cur = minHeap.Pop();
            if (cur.Row == n - 1 && cur.Col == m - 1) return cur.Cost;
            Add(map, cur.Row - 1, cur.Col, cur.Cost, minHeap, visited);
            Add(map, cur.Row + 1, cur.Col, cur.Cost, minHeap, visited);
            Add(map, cur.Row, cur.Col - 1, cur.Cost, minHeap, visited);
            Add(map, cur.Row, cur.Col + 1, cur.Cost, minHeap, visited);
        }

        return -1;
    }

    private static void Add(int[,] m, int i, int j, int pre, Heap<Node> heap, bool[,] visited)
    {
        if (i >= 0 && i < m.Length && j >= 0 && j < m.GetLength(1) && m[i, j] != 2 && !visited[i, j])
        {
            heap.Push(new Node(i, j, pre + (m[i, j] == 0 ? 2 : 1)));
            visited[i, j] = true;
        }
    }

    private class Node(int a, int b, int c)
    {
        public readonly int Col = b;
        public readonly int Cost = c;
        public readonly int Row = a;
    }
}