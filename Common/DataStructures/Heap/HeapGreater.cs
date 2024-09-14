namespace Common.DataStructures.Heap;

/// <summary>
///     加强堆
/// </summary>
/// <typeparam name="T">T一定要是非基础类型，有基础类型需求包一层</typeparam>
public class HeapGreater<T>(Func<T, T, int> comparison) where T : notnull
{
    private readonly List<T> _elements = [];
    private readonly Dictionary<T, int> _indexDict = new();
    public int Count { get; private set; }
    public bool IsEmpty=> Count == 0;
    private Func<T, T, int> Comparison { get; } = comparison ?? throw new ArgumentNullException(nameof(comparison));

    private int Compare(T a, T b)
    {
        return Comparison(a, b);
    }
    
    public bool Contains(T obj)
    {
        return _indexDict.ContainsKey(obj);
    }

    public T Peek()
    {
        return _elements[0];
    }

    public void Push(T value)
    {
        _elements.Add(value);
        _indexDict.Add(value, Count);
        HeapInsert(Count++);
    }

    public T Pop()
    {
        var ans = _elements[0];
        Swap(0, Count - 1);
        _indexDict.Remove(ans);
        _elements.RemoveAt(--Count);
        Heapify(0);
        return ans;
    }

    public void Remove(T obj)
    {
        var replace = _elements[Count - 1];
        var index = _indexDict[obj];
        _indexDict.Remove(obj);
        _elements.RemoveAt(--Count);
        if (!obj.Equals(replace))
        {
            _elements[index] = replace;
            _indexDict[replace] = index;
            Resign(replace);
        }
    }

    public void Resign(T obj)
    {
        HeapInsert(_indexDict[obj]);
        Heapify(_indexDict[obj]);
    }

    // 请返回堆上的所有元素
    public List<T> GetAllElements()
    {
        List<T> ans = new();
        foreach (var c in _elements)
            ans.Add(c);

        return ans;
    }

    private void HeapInsert(int index)
    {
        while (Compare(_elements[index], _elements[(index - 1) / 2]) < 0)
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
            var right = left + 1;
            var swappedIndex = right < Count && Compare(_elements[right], _elements[left]) < 0 ? right : left;
            swappedIndex = Compare(_elements[swappedIndex], _elements[index]) < 0 ? swappedIndex : index;
            if (swappedIndex == index) break;

            Swap(swappedIndex, index);
            index = swappedIndex;
            left = index * 2 + 1;
        }
    }

    private void Swap(int i, int j)
    {
        var o1 = _elements[i];
        var o2 = _elements[j];
        _elements[i] = o2;
        _elements[j] = o1;
        _indexDict[o2] = i;
        _indexDict[o1] = j;
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
        while (!minHeap.IsEmpty) Console.WriteLine($"minHeap.Pop:{minHeap.Pop()}");

        Console.WriteLine("大根堆测试:");

        var maxHeap = new HeapGreater<int>((x, y) => y.CompareTo(x));
        maxHeap.Push(1);
        maxHeap.Push(2);
        maxHeap.Push(3);
        maxHeap.Push(4);
        Console.WriteLine($"maxHeap.Count:{maxHeap.Count}");
        maxHeap.Push(5);
        while (!maxHeap.IsEmpty) Console.WriteLine($"maxHeap.Pop:{maxHeap.Pop()}");
    }
}