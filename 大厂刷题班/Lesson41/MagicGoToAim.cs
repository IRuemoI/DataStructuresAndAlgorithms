#region

using Common.DataStructures.Heap;

#endregion

namespace AdvancedTraining.Lesson41;

// 来自网易互娱
// N个结点之间，表世界存在双向通行的道路，里世界存在双向通行的传送门.
// 若走表世界的道路，花费一分钟.
// 若走里世界的传送门，不花费时间，但是接下来一分钟不能走传送门.
// 输入： T为测试用例的组数，对于每组数据:
// 第一行：N M1 M2 N代表结点的个数1到N
// 接下来M1行 每行两个数，u和v，表示表世界u和v之间存在道路
// 接下来M2行 每行两个数，u和v，表示里世界u和v之间存在传送门
// 现在处于1号结点，最终要到达N号结点，求最小的到达时间 保证所有输入均有效，不存在环等情况 
public class MagicGoToAim
{
    // 城市编号从0开始，编号对应0~n-1
    // roads[i]是一个数组，表示i能走路达到的城市有哪些，每条路都花费1分钟
    // gates[i]是一个数组，表示i能传送达到的城市有哪些
    // 返回从0到n-1的最少用时
    private static int Fast(int n, int[][] roads, int[][] gates)
    {
        int[][] distance = [new int[n], new int[n]];
        // 因为从0开始走，所以distance[0][0] = 0, distance[1][0] = 0
        // distance[0][i] -> 0 : 前一个城市到达i，是走路的方式, 最小代价，distance[0][i]
        // distance[1][i] -> 1 : 前一个城市到达i，是传送的方式, 最小代价，distance[1][i]
        for (var i = 1; i < n; i++)
        {
            distance[0][i] = int.MaxValue;
            distance[1][i] = int.MaxValue;
        }

        // 小根堆，根据距离排序，距离小的点，在上！
        var heap = new Heap<Node>((a, b) => a.Cost - b.Cost);
        heap.Push(new Node(0, 0, 0));
        bool[][] visited = [new bool[n], new bool[n]];
        while (!heap.isEmpty)
        {
            var cur = heap.Pop();
            if (visited[cur.PreTransfer][cur.City]) continue;
            visited[cur.PreTransfer][cur.City] = true;
            // 走路的方式
            foreach (var next in roads[cur.City])
                if (distance[0][next] > cur.Cost + 1)
                {
                    distance[0][next] = cur.Cost + 1;
                    heap.Push(new Node(0, next, distance[0][next]));
                }

            // 传送的方式
            if (cur.PreTransfer == 0)
                foreach (var next in gates[cur.City])
                    if (distance[1][next] > cur.Cost)
                    {
                        distance[1][next] = cur.Cost;
                        heap.Push(new Node(1, next, distance[1][next]));
                    }
        }

        return Math.Min(distance[0][n - 1], distance[1][n - 1]);
    }

    private class Node(int a, int b, int c)
    {
        public readonly int City = b;
        public readonly int Cost = c;
        public readonly int PreTransfer = a;
    }
}