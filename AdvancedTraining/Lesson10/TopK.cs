namespace AdvancedTraining.Lesson10;

// 本题测试链接：https://www.lintcode.com/problem/top-k-frequent-words-ii/
// 以上的代码不要粘贴, 把以下的代码粘贴进java环境编辑器
// 把类名和构造方法名改成TopK, 可以直接通过
public class TopK(int k)
{
    private readonly NodeHeapComp _comp = new();

    private readonly Node[] _heap = new Node[k];

    private readonly Dictionary<Node, int> _nodeIndexMap = new();

    // 词频表   key  abc   value  (abc,7)
    private readonly Dictionary<string, Node> _strNodeMap = new();
    private readonly SortedSet<Node> _treeSet = new(new NodeTreeSetComp());
    private int _heapSize;

    public void Add(string str)
    {
        if (_heap.Length == 0) return;
        // str   找到对应节点  curNode
        Node? curNode = null;
        // 对应节点  curNode  在堆上的位置
        var preIndex = -1;
        if (!_strNodeMap.TryGetValue(str, out var value))
        {
            curNode = new Node(str, 1);
            _strNodeMap[str] = curNode;
            _nodeIndexMap[curNode] = -1;
        }
        else
        {
            curNode = value;
            // 要在time++之前，先在treeSet中删掉
            // 原因是因为一但times++，curNode在treeSet中的排序就失效了
            // 这种失效会导致整棵treeSet出现问题
            if (_treeSet.Contains(curNode)) _treeSet.Remove(curNode);
            curNode.Times++;
            preIndex = _nodeIndexMap[curNode];
        }

        if (preIndex == -1)
        {
            if (_heapSize == _heap.Length)
            {
                if (_comp.Compare(_heap[0], curNode) < 0)
                {
                    _treeSet.Remove(_heap[0]);
                    _treeSet.Add(curNode);
                    _nodeIndexMap[_heap[0]] = -1;
                    _nodeIndexMap[curNode] = 0;
                    _heap[0] = curNode;
                    Heapify(0, _heapSize);
                }
            }
            else
            {
                _treeSet.Add(curNode);
                _nodeIndexMap[curNode] = _heapSize;
                _heap[_heapSize] = curNode;
                HeapInsert(_heapSize++);
            }
        }
        else
        {
            _treeSet.Add(curNode);
            Heapify(preIndex, _heapSize);
        }
    }

    public virtual IList<string> topk()
    {
        var ans = new List<string>();
        foreach (var node in _treeSet) ans.Add(node.Str);
        return ans;
    }

    private void HeapInsert(int index)
    {
        while (index != 0)
        {
            var parent = (index - 1) / 2;
            if (_comp.Compare(_heap[index], _heap[parent]) < 0)
            {
                Swap(parent, index);
                index = parent;
            }
            else
            {
                break;
            }
        }
    }

    private void Heapify(int index, int heapSize)
    {
        var l = index * 2 + 1;
        var r = index * 2 + 2;
        var smallest = index;
        while (l < heapSize)
        {
            if (_comp.Compare(_heap[l], _heap[index]) < 0) smallest = l;
            if (r < heapSize && _comp.Compare(_heap[r], _heap[smallest]) < 0) smallest = r;
            if (smallest != index)
                Swap(smallest, index);
            else
                break;
            index = smallest;
            l = index * 2 + 1;
            r = index * 2 + 2;
        }
    }

    private void Swap(int index1, int index2)
    {
        _nodeIndexMap[_heap[index1]] = index2;
        _nodeIndexMap[_heap[index2]] = index1;
        (_heap[index1], _heap[index2]) = (_heap[index2], _heap[index1]);
    }

    private class Node(string s, int t)
    {
        public readonly string Str = s;
        public int Times = t;
    }

    private class NodeHeapComp : IComparer<Node>
    {
        public int Compare(Node? o1, Node? o2)
        {
            return o1!.Times != o2!.Times
                ? o1.Times - o2.Times
                : string.Compare(o2.Str, o1.Str, StringComparison.Ordinal);
        }
    }

    private class NodeTreeSetComp : IComparer<Node>
    {
        public int Compare(Node? o1, Node? o2)
        {
            return o1!.Times != o2!.Times
                ? o2.Times - o1.Times
                : string.Compare(o1.Str, o2.Str, StringComparison.Ordinal);
        }
    }
}