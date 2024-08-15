namespace Common.DataStructures.Heap;

public class Heap<T>
{
    private readonly int _capacity;

    private readonly T[] _heap;
    //private readonly HeapType _heapType;

    /// <summary>
    ///     初始化堆
    /// </summary>
    /// <param name="comparison">对象比较表达式</param>
    /// <param name="capacity">堆最大容量，默认200</param>
    public Heap(Func<T, T, int> comparison, int capacity = 200)
    {
        _heap = new T[capacity];
        Count = 0;
        Comparison = comparison ?? throw new ArgumentNullException(nameof(comparison));
        _capacity = capacity;
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

    public bool IsFull()
    {
        return Count == _capacity;
    }

    public void Push(T value)
    {
        if (Count == _capacity) throw new Exception("heap is full");

        _heap[Count] = value;
        HeapInsert(_heap, Count++);
    }

    public T Pop()
    {
        var ans = _heap[0];
        Swap(_heap, 0, --Count);
        Heapify(_heap, 0, Count);
        return ans;
    }

    private void HeapInsert(T[] arr, int index)
    {
        while (Compare(arr[index], arr[(index - 1) / 2]) < 0)
        {
            Swap(arr, index, (index - 1) / 2);
            index = (index - 1) / 2;
        }
    }


    private void Heapify(T[] arr, int index, int heapSize)
    {
        var left = index * 2 + 1;

        while (left < heapSize)
        {
            // 如果有左孩子，有没有右孩子，可能有可能没有！
            // 把较小孩子的下标，给smallest
            var smallest = left + 1 < heapSize && Compare(arr[left + 1], arr[left]) < 0 ? left + 1 : left;
            smallest = Compare(arr[smallest], arr[index]) < 0 ? smallest : index;
            if (smallest == index) break;

            // index和较小孩子，要互换
            Swap(arr, smallest, index);
            index = smallest;
            left = index * 2 + 1;
        }
    }


    private static void Swap(IList<T> arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    public T Peek()
    {
        return _heap[0];
    }
}

public static class HeapTest
{
    public static void Run()
    {
        Console.WriteLine("小根堆测试:");
        var minHeap = new Heap<int>((x, y) => x.CompareTo(y));
        minHeap.Push(5);
        minHeap.Push(4);
        Console.WriteLine($"minHeap.Count:{minHeap.Count}");
        minHeap.Push(3);
        minHeap.Push(2);
        minHeap.Push(1);
        while (!minHeap.IsEmpty()) Console.WriteLine($"minHeap.Pop:{minHeap.Pop()}");

        Console.WriteLine("大根堆测试:");

        var maxHeap = new Heap<int>((x, y) => y.CompareTo(x));
        maxHeap.Push(1);
        maxHeap.Push(2);
        maxHeap.Push(3);
        maxHeap.Push(4);
        Console.WriteLine($"maxHeap.Count:{maxHeap.Count}");
        maxHeap.Push(5);
        while (!maxHeap.IsEmpty()) Console.WriteLine($"maxHeap.Pop:{maxHeap.Pop()}");
    }
}

//以下堆继承自实现了比较器的接口但存在的一些问题，当被继承的类需要多个比较器时，被继承的类只能实现一个比较器
//如果从始至终只有一个比较标准可以使用以下方式

// public class MinHeap<T> where T : IComparable<T>
// {
//     private readonly T[] _heap;
//     private readonly int _capacity;
//     private int Count { get; set; }
//
//     public MinHeap(int capacity)
//     {
//         _heap = new T[capacity];
//         _capacity = capacity;
//         Count = 0;
//     }
//
//     public bool IsEmpty()
//     {
//         return Count == 0;
//     }
//
//     public bool IsFull()
//     {
//         return Count == _capacity;
//     }
//
//     public void Push(T value)
//     {
//         if (Count == _capacity)
//         {
//             throw new Exception("heap is full");
//         }
//
//         _heap[Count] = value;
//         HeapInsert(_heap, Count++);
//     }
//
//     public T Pop()
//     {
//         T ans = _heap[0];
//         Swap(_heap, 0, --Count);
//         Heapify(_heap, 0, Count);
//         return ans;
//     }
//
//     private void HeapInsert(T[] arr, int index)
//     {
//         while (arr[index].CompareTo(arr[(index - 1) / 2]) < 0)
//         {
//             Swap(arr, index, (index - 1) / 2);
//             index = (index - 1) / 2;
//         }
//     }
//
//     private void Heapify(T[] arr, int index, int heapSize)
//     {
//         int left = index * 2 + 1;
//         while (left < heapSize)
//         {
//             // 如果有左孩子，有没有右孩子，可能有可能没有！
//             // 把较小孩子的下标，给smallest
//             int smallest = left + 1 < heapSize && arr[left + 1].CompareTo(arr[left]) < 0 ? left + 1 : left;
//             smallest = arr[smallest].CompareTo(arr[index]) < 0 ? smallest : index;
//             if (smallest == index)
//             {
//                 break;
//             }
//
//             // index和较小孩子，要互换
//             Swap(arr, smallest, index);
//             index = smallest;
//             left = index * 2 + 1;
//         }
//     }
//
//     private static void Swap(T[] arr, int i, int j)
//     {
//         (arr[i], arr[j]) = (arr[j], arr[i]);
//     }
//
//     public T Peek()
//     {
//         return _heap[0];
//     }
//}