//测试通过

#region

using Common.DataStructures.Graph;

#endregion

namespace Algorithms.Lesson16;

// https://www.bilibili.com/video/BV1Cm4y1g77W
// 无负权边
public static class Dijkstra
{
    public static Dictionary<Dot, int> Dijkstra1(Dot from)
    {
        // 记录所有点的最小距离，key是点的对象，value是开始节点到该点的最小距离，如果查不到那么默认为正无穷大
        Dictionary<Dot, int> distanceMap = new() { { from, 0 } };
        // 打过对号的点，也就是已经确定最短距离的点
        HashSet<Dot?> selectedNodes = new();
        var minNode = GetMinDistanceAndUnselectedNode(distanceMap, selectedNodes);
        while (minNode != null)
        {
            //  原始点  ->  minNode(跳转点)   最小距离distance
            var distance = distanceMap[minNode];
            foreach (var edge in minNode.BelongEdgeList)
            {
                var toNode = edge.To;
                if (toNode != null)
                {
                    if (!distanceMap.ContainsKey(toNode))
                        distanceMap[toNode] = distance + edge.Weight;
                    else
                        // toNode 
                        distanceMap[toNode] = Math.Min(distanceMap[toNode], distance + edge.Weight);
                }
                else
                {
                    throw new AggregateException();
                }
            }

            selectedNodes.Add(minNode);
            minNode = GetMinDistanceAndUnselectedNode(distanceMap, selectedNodes);
        }

        return distanceMap;
    }

    private static Dot? GetMinDistanceAndUnselectedNode(Dictionary<Dot, int> distanceMap, HashSet<Dot?> touchedNodes)
    {
        Dot? minNode = null;
        var minDistance = int.MaxValue;
        foreach (var entry in distanceMap)
        {
            var node = entry.Key;
            var distance = entry.Value;
            if (!touchedNodes.Contains(node) && distance < minDistance)
            {
                minNode = node;
                minDistance = distance;
            }
        }

        return minNode;
    }

    // 使用加强堆改进后的dijkstra算法
    // 从head出发，所有head能到达的节点，生成到达每个节点的最小路径记录并返回
    // 使用加强堆解决每次遍历都要遍历所有点的问题
    public static Dictionary<Dot, int> Dijkstra2(Dot head, int size)
    {
        var nodeHeap = new NodeHeap(size);
        nodeHeap.AddOrUpdateOrIgnore(head, 0);
        Dictionary<Dot, int> result = new();
        while (!nodeHeap.IsEmpty())
        {
            var record = nodeHeap.Pop();
            var cur = record.Node;
            var distance = record.Distance;
            if (cur != null)
            {
                if (cur.BelongEdgeList.Count > 0)
                    foreach (var edge in cur.BelongEdgeList)
                        nodeHeap.AddOrUpdateOrIgnore(edge.To ?? throw new InvalidOperationException(),
                            edge.Weight + distance);

                result.Add(cur, distance);
            }
        }

        return result;
    }

    public static void Run()
    {
        int[,] matrix =
        {
            { 5, 0, 1 },
            { 2, 0, 2 },
            { 8, 2, 5 },
            { 6, 2, 3 },
            { 1, 1, 3 },
            { 6, 1, 4 },
            { 2, 3, 5 },
            { 1, 3, 4 },
            { 3, 5, 6 },
            { 7, 4, 6 }
        };
        var graph = GraphGenerator.CreateGraph(matrix, GraphType.Directed);
        Console.WriteLine("迪杰斯特拉算法1:");
        foreach (var keyValuePair in Dijkstra1(graph.Dots[0]!).OrderBy(x => x.Key.Label))
            Console.WriteLine(keyValuePair.Key.Label + " : " + keyValuePair.Value);

        Console.WriteLine("迪杰斯特拉算法2:");
        foreach (var keyValuePair in Dijkstra2(graph.Dots[0]!, 7).OrderBy(x => x.Key.Label))
            Console.WriteLine(keyValuePair.Key.Label + " : " + keyValuePair.Value);
    }

    private class NodeRecord(Dot? node, int distance)
    {
        public readonly int Distance = distance;
        public readonly Dot? Node = node;
    }

    private class NodeHeap(int size)
    {
        // key 某一个节点， value 从源节点出发到该节点的目前最小距离
        private readonly Dictionary<Dot, int> _distanceMap = new();

        // key 某一个node， value 上面堆中的位置
        private readonly Dictionary<Dot, int> _heapIndexMap = new();
        private readonly Dot?[] _nodes = new Dot?[size]; // 实际的堆结构
        private int _size; // 堆上有多少个点，初始size = 0;

        public bool IsEmpty()
        {
            return _size == 0;
        }

        // 有一个点叫node，现在发现了一个从源节点出发到达node的距离为distance
        // 判断要不要更新，如果需要的话，就更新
        public void AddOrUpdateOrIgnore(Dot node, int distance)
        {
            if (InHeap(node))
            {
                _distanceMap[node] = Math.Min(_distanceMap[node], distance);
                InsertHeapify(_heapIndexMap[node]);
            }

            if (!IsEntered(node))
            {
                _nodes[_size] = node;
                _heapIndexMap.Add(node, _size);
                _distanceMap[node] = distance;
                InsertHeapify(_size++);
            }
        }

        public NodeRecord Pop()
        {
            var nodeRecord =
                new NodeRecord(_nodes[0], _distanceMap[_nodes[0] ?? throw new InvalidOperationException()]);
            Swap(0, _size - 1);
            _heapIndexMap[_nodes[_size - 1] ?? throw new InvalidOperationException()] = -1;
            _distanceMap.Remove(_nodes[_size - 1] ?? throw new InvalidOperationException());
            // free C++同学还要把原本堆顶节点析构，对java同学不必
            _nodes[_size - 1] = null;
            Heapify(0, --_size);
            return nodeRecord;
        }

        private void InsertHeapify(int index)
        {
            while (_distanceMap[_nodes[index] ?? throw new InvalidOperationException()] <
                   _distanceMap[_nodes[(index - 1) / 2] ?? throw new InvalidOperationException()])
            {
                Swap(index, (index - 1) / 2);
                index = (index - 1) / 2;
            }
        }

        private void Heapify(int index, int size)
        {
            var left = index * 2 + 1;
            while (left < size)
            {
                var smallest = left + 1 < size &&
                               _distanceMap[_nodes[left + 1] ?? throw new InvalidOperationException()]
                               <
                               _distanceMap[_nodes[left] ?? throw new InvalidOperationException()]
                    ? left + 1
                    : left;
                smallest = _distanceMap[_nodes[smallest] ?? throw new InvalidOperationException()]
                           < _distanceMap[_nodes[index] ?? throw new InvalidOperationException()]
                    ? smallest
                    : index;
                if (smallest == index) break;

                Swap(smallest, index);
                index = smallest;
                left = index * 2 + 1;
            }
        }

        private bool IsEntered(Dot node)
        {
            return _heapIndexMap.ContainsKey(node);
        }

        private bool InHeap(Dot node)
        {
            return IsEntered(node) && _heapIndexMap[node] != -1;
        }

        private void Swap(int index1, int index2)
        {
            _heapIndexMap[_nodes[index1] ?? throw new InvalidOperationException()] = index2;
            _heapIndexMap[_nodes[index2] ?? throw new InvalidOperationException()] = index1;
            (_nodes[index1], _nodes[index2]) = (_nodes[index2], _nodes[index1]);
        }
    }
}