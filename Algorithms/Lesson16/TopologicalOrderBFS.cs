namespace Algorithms.Lesson16;

// OJ链接：https://www.lintcode.com/problem/topological-sorting

public static class TopologicalOrderBfs
{
    public static List<DirectedGraphNode> TopSort(List<DirectedGraphNode> graph)
    {
        var order = new Dictionary<DirectedGraphNode, Record>();
        foreach (var cur in graph) Process(cur, order);

        var recordArr = new List<Record>();
        foreach (var r in order.Values) recordArr.Add(r);

        recordArr.Sort((x, y) => y.Deep - x.Deep);
        var ans = new List<DirectedGraphNode>();
        foreach (var r in recordArr) ans.Add(r.Node);

        return ans;
    }

    private static Record Process(DirectedGraphNode cur, Dictionary<DirectedGraphNode, Record> order)
    {
        if (order.TryGetValue(cur, out var f1)) return f1;

        var follow = 0;
        foreach (var next in cur.Neighbors) follow = Math.Max(follow, Process(next, order).Deep);

        var ans = new Record(cur, follow + 1);
        order[cur] = ans;
        return ans;
    }

    // 不要提交这个类
    public class DirectedGraphNode
    {
        public readonly int Label;
        public readonly List<DirectedGraphNode> Neighbors;

        public DirectedGraphNode(int x)
        {
            Label = x;
            Neighbors = new List<DirectedGraphNode>();
        }
    }

// 不要提交这个类
    public class Record
    {
        public readonly int Deep;
        public readonly DirectedGraphNode Node;

        public Record(DirectedGraphNode n, int o)
        {
            Node = n;
            Deep = o;
        }
    }
}

public static class TopologicalOrderBfsTest
{
    public static void Run()
    {
        var node4 = new TopologicalOrderBfs.DirectedGraphNode(4);
        var node5 = new TopologicalOrderBfs.DirectedGraphNode(5);
        var graph = new List<TopologicalOrderBfs.DirectedGraphNode>
        {
            new(0)
            {
                Neighbors =
                {
                    new TopologicalOrderBfs.DirectedGraphNode(1)
                    {
                        Neighbors =
                        {
                            node4
                        }
                    },
                    new TopologicalOrderBfs.DirectedGraphNode(2)
                    {
                        Neighbors =
                        {
                            node4,
                            node5
                        }
                    },
                    new TopologicalOrderBfs.DirectedGraphNode(3)
                    {
                        Neighbors =
                        {
                            node4,
                            node5
                        }
                    }
                }
            }
        };
        Console.WriteLine("拓扑排序:");
        foreach (var item in TopologicalOrderBfs.TopSort(graph)) Console.WriteLine(item.Label);
    }
}