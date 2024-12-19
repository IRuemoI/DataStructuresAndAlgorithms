//测试通过

#region

using Common.DataStructures.Graph;
using Common.DataStructures.Heap;

#endregion

namespace Algorithms.Lesson16;

// undirected graph only
public class Prim
{
    public static HashSet<Edge> PrimMst(Graph graph)
    {
        // 解锁的边进入小根堆
        Heap<Edge> minHeap = new((x, y) => x.Weight - y.Weight);

        // 哪些点被解锁出来了
        HashSet<Dot?> nodeSet = new();


        HashSet<Edge> result = new(); // 依次挑选的的边在result里

        foreach (var node in graph.Dots.Values)
            // 随便挑了一个点
            // node 是开始点
            if (nodeSet.Add(node))
                if (node?.BelongEdgeList.Count > 0)
                {
                    foreach (var edge in node.BelongEdgeList)
                        // 由一个点，解锁所有相连的边
                        minHeap.Push(edge);

                    while (minHeap.count != 0)
                    {
                        var edge = minHeap.Pop(); // 弹出解锁的边中，最小的边
                        var toNode = edge.To; // 可能的一个新的点
                        if (nodeSet.Add(toNode))
                        {
                            // 不含有的时候，就是新的点
                            result.Add(edge);
                            if (toNode?.BelongEdgeList != null)
                                foreach (var nextEdge in toNode.BelongEdgeList)
                                    minHeap.Push(nextEdge);
                        }
                    }
                }

        // break;
        return result;
    }

    // 请保证graph是连通图
    // graph[i][j]表示点i到点j的距离，如果是系统最大值代表无路
    // 返回值是最小连通图的路径之和
    public static int Code(int[][] graph)
    {
        var size = graph.Length;
        var distances = new int[size];
        var visit = new bool[size];
        visit[0] = true;
        for (var i = 0; i < size; i++) distances[i] = graph[0][i];

        var sum = 0;
        for (var i = 1; i < size; i++)
        {
            var minPath = int.MaxValue;
            var minIndex = -1;
            for (var j = 0; j < size; j++)
                if (!visit[j] && distances[j] < minPath)
                {
                    minPath = distances[j];
                    minIndex = j;
                }

            if (minIndex == -1) return sum;

            visit[minIndex] = true;
            sum += minPath;
            for (var j = 0; j < size; j++)
                if (!visit[j] && distances[j] > graph[minIndex][j])
                    distances[j] = graph[minIndex][j];
        }

        return sum;
    }

    public static void Run()
    {
        int[,] matrix =
        {
            { 1, 0, 3 },
            { 2, 0, 1 },
            { 3, 0, 2 },
            { 4, 1, 2 },
            { 5, 1, 3 },
            { 6, 2, 3 }
        };
        var graph = GraphGenerator.CreateGraph(matrix, GraphType.Undirected);
        Console.WriteLine("普利姆算法:");
        foreach (var edge in Kruskal.KruskalMst(graph))
            Console.WriteLine(edge.From?.Label + " -> " + edge.To?.Label + " : " + edge.Weight);
    }
}