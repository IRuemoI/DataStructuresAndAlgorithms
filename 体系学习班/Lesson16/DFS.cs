//测试通过

#region

using Common.DataStructures.Graph;

#endregion

namespace Algorithms.Lesson16;

public static class Dfs
{
    public static void Code(Dot? node)
    {
        if (node == null) return;

        Stack<Dot?> stack = new();
        HashSet<Dot?> set = new();
        stack.Push(node);
        set.Add(node);
        Console.WriteLine($"node.Value:{node.Label}");
        while (stack.Count != 0)
        {
            var cur = stack.Pop();
            if (cur?.PointToList.Count == 0) continue;
            if (cur?.PointToList != null)
                foreach (var next in cur.PointToList)
                {
                    if (set.Contains(next)) continue;
                    stack.Push(cur);
                    stack.Push(next);
                    set.Add(next);
                    if (next != null) Console.WriteLine($"node.Value:{next.Label}");

                    break;
                }
        }
    }
}

public static class DfsTest
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
        Console.WriteLine("深度优先遍历:");
        Dfs.Code(graph.Dots[0]);
    }
}