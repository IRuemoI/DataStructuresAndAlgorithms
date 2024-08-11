namespace Common.DataStructures.Heap;

/// <summary>
///     加强堆
/// </summary>
/// <typeparam name="T">T一定要是非基础类型，有基础类型需求包一层</typeparam>
public class HeapGreater<T> where T : notnull
{
    private readonly List<T> _heap;
    private readonly Dictionary<T, int> _indexMap;

    public HeapGreater(Func<T, T, int> comparison)
    {
        _heap = new List<T>();
        _indexMap = new Dictionary<T, int>();
        Count = 0;
        Comparison = comparison ?? throw new ArgumentNullException(nameof(comparison));
    }

    public int Count { get; private set; }
    private Func<T, T, int> Comparison { get; }

    private int Compare(T a, T b)
    {
        return Comparison(a, b);
    }

    public bool IsEmpty()
    {
        return Count == 0;
    }

    public int Size()
    {
        return Count;
    }

    public bool Contains(T obj)
    {
        return _indexMap.ContainsKey(obj);
    }

    public T Peek()
    {
        return _heap[0];
    }

    public void Push(T value)
    {
        _heap.Add(value);
        _indexMap.Add(value, Count);
        HeapInsert(Count++);
    }

    public T Pop()
    {
        var ans = _heap[0];
        Swap(0, Count - 1);
        _indexMap.Remove(ans);
        _heap.RemoveAt(--Count);
        Heapify(0);
        return ans;
    }

    public void Remove(T obj)
    {
        var replace = _heap[Count - 1];
        var index = _indexMap[obj];
        _indexMap.Remove(obj);
        _heap.RemoveAt(--Count);
        if (!obj.Equals(replace))
        {
            _heap[index] = replace;
            _indexMap[replace] = index;
            Resign(replace);
        }
    }

    public void Resign(T obj)
    {
        HeapInsert(_indexMap[obj]);
        Heapify(_indexMap[obj]);
    }

    // 请返回堆上的所有元素
    public List<T> GetAllElements()
    {
        List<T> ans = new();
        foreach (var c in _heap)
            ans.Add(c);

        return ans;
    }

    private void HeapInsert(int index)
    {
        while (Compare(_heap[index], _heap[(index - 1) / 2]) < 0)
        {
            Swap(index, (index - 1) / 2);
            index = (index - 1) / 2;
        }
    }

    private void Heapify(int index)
    {
        var left = index * 2 + 1;


        while (left < Count)
        {
            var smallest = left + 1 < Count && Compare(_heap[left + 1], _heap[left]) < 0 ? left + 1 : left;
            smallest = Compare(_heap[smallest], _heap[index]) < 0 ? smallest : index;
            if (smallest == index) break;

            Swap(smallest, index);
            index = smallest;
            left = index * 2 + 1;
        }
    }

    private void Swap(int i, int j)
    {
        var o1 = _heap[i];
        var o2 = _heap[j];
        _heap[i] = o2;
        _heap[j] = o1;
        _indexMap[o2] = i;
        _indexMap[o1] = j;
    }
}

public static class HeapGreater
{
    public static void Run()
    {
        Console.WriteLine("加强堆:");
        Console.WriteLine("小根堆测试:");
        var minHeap = new HeapGreater<int>((x, y) => x.CompareTo(y));
        minHeap.Push(5);
        minHeap.Push(4);
        Console.WriteLine($"minHeap.Count:{minHeap.Count}");
        minHeap.Push(3);
        minHeap.Push(2);
        minHeap.Push(1);
        while (!minHeap.IsEmpty()) Console.WriteLine($"minHeap.Pop:{minHeap.Pop()}");

        Console.WriteLine("大根堆测试:");

        var maxHeap = new HeapGreater<int>((x, y) => y.CompareTo(x));
        maxHeap.Push(1);
        maxHeap.Push(2);
        maxHeap.Push(3);
        maxHeap.Push(4);
        Console.WriteLine($"maxHeap.Count:{maxHeap.Count}");
        maxHeap.Push(5);
        while (!maxHeap.IsEmpty()) Console.WriteLine($"maxHeap.Pop:{maxHeap.Pop()}");
    }
}