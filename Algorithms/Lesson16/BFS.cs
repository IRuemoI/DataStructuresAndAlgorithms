//测试通过

#region

using Common.DataStructures.Graph;

#endregion

namespace Algorithms.Lesson16;

public static class Bfs
{
    // 从node出发，进行宽度优先遍历
    public static void Code(Dot? start)
    {
        if (start == null) return;

        Queue<Dot?> queue = new();
        HashSet<Dot?> set = new();
        queue.Enqueue(start);
        set.Add(start);
        while (queue.Count != 0)
        {
            var cur = queue.Dequeue();
            if (cur != null)
            {
                Console.WriteLine($"node.Value:{cur.Label}");
                foreach (var next in cur.PointToList)
                    if (!set.Contains(next))
                    {
                        set.Add(next);
                        queue.Enqueue(next);
                    }
            }
        }
    }
}

public static class BfsTest
{
    public static void Run()
    {
        int[,] matrix =
        {
            { 0, 0, 3 },
            { 0, 0, 1 },
            { 0, 0, 2 },
            { 0, 1, 2 },
            { 0, 1, 3 },
            { 0, 2, 3 }
        };
        var graph = GraphGenerator.CreateGraph(matrix, GraphType.Directed);
        Console.WriteLine("广度优先遍历:");
        Bfs.Code(graph.Dots[0]);
    }
}