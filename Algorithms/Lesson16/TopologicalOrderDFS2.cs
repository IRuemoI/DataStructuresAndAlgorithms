//测试通过

namespace Algorithms.Lesson16;

// OJ链接：https://www.lintcode.com/problem/topological-sorting
public abstract class TopologicalOrderDfs2
{
    public static List<DirectedGraphNode> TopSort(List<DirectedGraphNode> graph)
    {
        Dictionary<DirectedGraphNode, Record> order = new();
        foreach (var cur in graph) GetInfo(cur, order);

        List<Record> recordArr = new();
        foreach (var r in
                 order.Values)
            recordArr.Add(r);
        recordArr.Sort((x, y) => x.Nodes == y.Nodes ? 0 : x.Nodes > y.Nodes ? -1 : 1);
        var ans = new List<DirectedGraphNode>();
        foreach (var r in recordArr) ans.Add(r.Node);

        return ans;
    }

    // 当前来到cur点，请返回cur点所到之处，所有的点次！
    // 返回（cur，点次）
    // 缓存！！！！！order   
    //  key : 某一个点的点次，之前算过了！
    //  value : 点次是多少
    private static Record GetInfo(DirectedGraphNode cur, Dictionary<DirectedGraphNode, Record> order)
    {
        if (order.TryGetValue(cur, out var info)) return info;

        // cur的点次之前没算过！
        long nodes = 0;
        foreach (var next in cur.Neighbors) nodes += GetInfo(next, order).Nodes;

        var ans = new Record(cur, nodes + 1);
        order.Add(cur, ans);
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
        public readonly DirectedGraphNode Node;
        public readonly long Nodes;

        public Record(DirectedGraphNode n, long o)
        {
            Node = n;
            Nodes = o;
        }
    }
}

public static class TopologicalOrderDfs2Test
{
    public static void Run()
    {
        var node4 = new TopologicalOrderDfs2.DirectedGraphNode(4);
        var node5 = new TopologicalOrderDfs2.DirectedGraphNode(5);
        var graph = new List<TopologicalOrderDfs2.DirectedGraphNode>
        {
            new(0)
            {
                Neighbors =
                {
                    new TopologicalOrderDfs2.DirectedGraphNode(1)
                    {
                        Neighbors =
                        {
                            node4
                        }
                    },
                    new TopologicalOrderDfs2.DirectedGraphNode(2)
                    {
                        Neighbors =
                        {
                            node4,
                            node5
                        }
                    },
                    new TopologicalOrderDfs2.DirectedGraphNode(3)
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
        foreach (var item in TopologicalOrderDfs2.TopSort(graph)) Console.WriteLine(item.Label);
    }
}