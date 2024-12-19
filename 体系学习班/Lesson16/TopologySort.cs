#region

using Common.DataStructures.Graph;

#endregion

namespace Algorithms.Lesson16;

public static class TopologySort
{
    // directed graph and no loop
    public static List<Dot?> Code(Graph graph)
    {
        // key 某个节点   value 剩余的入度
        Dictionary<Dot, int> inMap = new();
        // 只有剩余入度为0的点，才进入这个队列
        Queue<Dot?> zeroInQueue = new();
        foreach (var node in graph.Dots.Values)
            if (node != null)
            {
                inMap.Add(node, node.InDegree);
                if (node.InDegree == 0) zeroInQueue.Enqueue(node);
            }

        List<Dot?> result = new();
        while (zeroInQueue.Count != 0)
        {
            var cur = zeroInQueue.Dequeue();
            result.Add(cur);
            if (cur != null)
                foreach (var next in cur.PointToList)
                {
                    inMap[next ?? throw new InvalidOperationException()] -= 1;
                    if (inMap[next] == 0) zeroInQueue.Enqueue(next);
                }
        }

        return result;
    }
}

public static class TopologySortTest
{
    public static void Run()
    {
        int[,] matrix =
        {
            { 0, 0, 1 },
            { 0, 0, 2 },
            { 0, 0, 3 },
            { 0, 1, 4 },
            { 0, 2, 4 },
            { 0, 2, 5 },
            { 0, 3, 4 },
            { 0, 3, 5 }
        };
        var graph = GraphGenerator.CreateGraph(matrix, GraphType.Directed);
        Console.WriteLine("拓扑排序:");
        foreach (var dot in TopologySort.Code(graph)) Console.WriteLine(dot?.Label);
    }
}