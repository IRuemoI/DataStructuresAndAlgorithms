//测试通过

#region

using Common.DataStructures.Graph;
using Common.DataStructures.Heap;

#endregion

namespace Algorithms.Lesson16;

//undirected graph only
public static class Kruskal
{
    //最小生成树：比如多个村庄使用公路连接，如何修筑公路使总成本最小。
    //每次选择权重最小的边，如果该边的两个顶点不在同一个子图，就选择这条边，否则就丢弃它。
    public static HashSet<Edge> KruskalMst(Graph graph)
    {
        var unionFind = new UnionFind();
        unionFind.MakeSets(graph.Dots.Values.ToList());
        var minHeap = new Heap<Edge>((x, y) => x.Weight - y.Weight);
        foreach (var edge in graph.Edges)
            // M 条边
            minHeap.Push(edge); // O(logM)

        HashSet<Edge> result = new();
        while (minHeap.Count != 0)
        {
            // M 条边
            var edge = minHeap.Pop(); // O(logM)
            if (edge is { From: not null, To: not null } && !unionFind.IsSameSet(edge.From, edge.To))
            {
                // O(1)
                result.Add(edge);
                unionFind.Union(edge.From, edge.To);
            }
        }

        return result;
    }

    // Union-Find Set
    private class UnionFind
    {
        // key 某一个节点， value key节点往上的节点
        private readonly Dictionary<Dot, Dot> _fatherMap;

        // key 某一个集合的代表节点, value key所在集合的节点个数
        private readonly Dictionary<Dot, int> _sizeMap;

        public UnionFind()
        {
            _fatherMap = new Dictionary<Dot, Dot>();
            _sizeMap = new Dictionary<Dot, int>();
        }

        public void MakeSets(List<Dot?> nodes)
        {
            _fatherMap.Clear();
            _sizeMap.Clear();
            foreach (var node in nodes)
            {
                _fatherMap[node ?? throw new InvalidOperationException()] = node;
                _sizeMap[node] = 1;
            }
        }

        private Dot FindFather(Dot n)
        {
            Stack<Dot> path = new();
            while (n != _fatherMap[n])
            {
                path.Push(n);
                n = _fatherMap[n];
            }

            while (path.Count != 0) _fatherMap[path.Pop()] = n;

            return n;
        }

        public bool IsSameSet(Dot a, Dot b)
        {
            return FindFather(a) == FindFather(b);
        }

        public void Union(Dot? a, Dot? b)
        {
            if (a == null || b == null) return;

            var aDai = FindFather(a);
            var bDai = FindFather(b);
            if (aDai != bDai)
            {
                var aSetSize = _sizeMap[aDai];
                var bSetSize = _sizeMap[bDai];
                if (aSetSize <= bSetSize)
                {
                    _fatherMap[aDai] = bDai;
                    _sizeMap[bDai] = aSetSize + bSetSize;
                    _sizeMap.Remove(aDai);
                }
                else
                {
                    _fatherMap[bDai] = aDai;
                    _sizeMap[aDai] = aSetSize + bSetSize;
                    _sizeMap.Remove(bDai);
                }
            }
        }
    }
}

public static class KruskalTest
{
    public static void Run()
    {
        //模拟无向图
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
        Console.WriteLine("克鲁斯卡尔算法:");
        foreach (var edge in Kruskal.KruskalMst(graph))
            Console.WriteLine(edge.From?.Label + " -> " + edge.To?.Label + " : " + edge.Weight);
    }
}