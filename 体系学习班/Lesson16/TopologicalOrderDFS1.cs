//测试通过

namespace Algorithms.Lesson16;

// OJ链接：https://www.lintcode.com/problem/topological-sorting
public abstract class TopologicalOrderDfs1
{
    public static List<DirectedGraphNode> TopSort(List<DirectedGraphNode> graph)
    {
        Dictionary<DirectedGraphNode, Record> order = new();
        foreach (var cur in graph) GetInfo(cur, order);

        List<Record> recordArr = new();
        foreach (var r in order.Values) recordArr.Add(r);

        recordArr.Sort((x, y) => y.Deep - x.Deep);
        var ans = new List<DirectedGraphNode>();
        foreach (var r in recordArr)
            if (!ans.Contains(r.Node))
                ans.Add(r.Node);

        return ans;
    }

    /// <summary>
    ///     当前来到cur点，返回cur,点次
    /// </summary>
    /// <param name="cur">查找的点</param>
    /// <param name="order">点次</param>
    /// <returns>cur,点次</returns>
    private static Record GetInfo(DirectedGraphNode cur, Dictionary<DirectedGraphNode, Record> order)
    {
        if (order.TryGetValue(cur, out var record)) return record;

        var follow = 0;
        foreach (var next in cur.Neighbors) follow = Math.Max(follow, GetInfo(next, order).Deep);

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

    // 提交下面的
    private class Record
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

public static class TopologicalOrderDfs1Test
{
    public static void Run()
    {
        var node4 = new TopologicalOrderDfs1.DirectedGraphNode(4);
        var node5 = new TopologicalOrderDfs1.DirectedGraphNode(5);
        var graph = new List<TopologicalOrderDfs1.DirectedGraphNode>
        {
            new(0)
            {
                Neighbors =
                {
                    new TopologicalOrderDfs1.DirectedGraphNode(1)
                    {
                        Neighbors =
                        {
                            node4
                        }
                    },
                    new TopologicalOrderDfs1.DirectedGraphNode(2)
                    {
                        Neighbors =
                        {
                            node4,
                            node5
                        }
                    },
                    new TopologicalOrderDfs1.DirectedGraphNode(3)
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


        foreach (var item in TopologicalOrderDfs1.TopSort(graph)) Console.WriteLine(item.Label);
    }
}