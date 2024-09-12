namespace Common.DataStructures.Heap;

public class Heap<T>
{
    private readonly int _capacity;
    private readonly T[] _elements;
    private Func<T, T, int> Comparison { get; }
    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;
    public bool IsFull => Count == _capacity;

    /// <summary>
    /// 初始化堆
    /// </summary>
    /// <param name="comparison">对象比较表达式</param>
    /// <param name="capacity">堆最大容量，默认200</param>
    public Heap(Func<T, T, int> comparison, int capacity = 200)
    {
        _elements = new T[capacity];
        Comparison = comparison ?? throw new ArgumentNullException(nameof(comparison));
        _capacity = capacity > 0 ? capacity : throw new ArgumentException(nameof(capacity));
        Count = 0;
    }

    private int Compare(T x, T y)
    {
        return Comparison(x, y);
    }

    public void Push(T value)
    {
        if (IsFull) throw new Exception("The heap is full");
        _elements[Count] = value; //将数据放到最后一个
        HeapInsert(Count); //将元素插入到合适的位置
        Count++; //增加堆内元素计数
    }

    public T Pop()
    {
        if (IsEmpty) throw new Exception("The heap is empty");
        var removedElement = _elements[0]; //移除堆顶元素
        Count--; //减少堆内元素计数
        (_elements[Count], _elements[0]) = (_elements[0], _elements[Count]); //将堆内最后一个元素移动到第一个位置
        Heapify(0); //将元素移动到合适的位置
        return removedElement; //返回被移除的元素
    }

    private void HeapInsert(int currentIndex)
    {
        //因为这个泛型的堆的比较表达式是不确定的(构成大根堆或者小根堆)，所以在此这么描述：
        //如果当前值还可以和它的父节点交换(比较表达式小于0表示当前节点不符合当前堆的规则)
        while (Compare(_elements[currentIndex], _elements[(currentIndex - 1) / 2]) < 0)
        {
            var parentIndex = (currentIndex - 1) / 2; //获得父节点的下标
            //交换当前节点和父节点
            (_elements[parentIndex], _elements[currentIndex]) = (_elements[currentIndex], _elements[parentIndex]); 
            currentIndex = (currentIndex - 1) / 2; //更新当前节点的下标
        }
    }

    private void Heapify(int currentIndex)
    {
        var leftChild = currentIndex * 2 + 1; //获取左子节点的下标
        
        while (leftChild < Count) //如果左子节点的下标还在堆内元素的范围内
        {
            var rightChild = leftChild + 1; //获取右子节点的下标
            //如果右子节点的下标在堆内元素的范围内且右子节点是需要交换的节点
            var swappedIndex = rightChild < Count && Compare(_elements[rightChild], _elements[leftChild]) < 0
                ? rightChild
                : leftChild;
            //在将被选择那一边子节点和当前节点进行比较，决定是否交换，如果不需要交换swappedIndex将被重新赋值为currentIndex
            swappedIndex = Compare(_elements[swappedIndex], _elements[currentIndex]) < 0 ? swappedIndex : currentIndex;
            if (swappedIndex == currentIndex) break; //如果被交换的节点就是当前节点，那么退出函数
            //和需要交换的节点进行交换
            (_elements[swappedIndex], _elements[currentIndex]) = (_elements[currentIndex], _elements[swappedIndex]); 
            currentIndex = swappedIndex; //更新当前节点所在的下标
            leftChild = currentIndex * 2 + 1; //更新左子节点的下标
        }
    }

    public T Peek()
    {
        return Count > 0 ? _elements[0] : throw new Exception("The heap is empty");
    }
}

public static class HeapTest
{
    public static void Run()
    {
        Console.WriteLine("大根堆");
        var maxHeap = new Heap<int>((x, y) => y.CompareTo(x), 10);
        //myMaxHeap.Peek();
        maxHeap.Push(4);
        maxHeap.Push(3);
        maxHeap.Push(2);
        maxHeap.Push(1);
        maxHeap.Push(0);
        maxHeap.Push(9);
        maxHeap.Push(8);
        maxHeap.Push(7);
        maxHeap.Push(6);
        maxHeap.Push(5);
        Console.WriteLine(maxHeap.IsFull);
        while (!maxHeap.IsEmpty) Console.WriteLine(maxHeap.Pop());
        //myMaxHeap.Pop();

        Console.WriteLine("小根堆");
        var minHeap = new Heap<int>((x, y) => x.CompareTo(y), 10);
        //myMinHeap.Peek();
        minHeap.Push(4);
        minHeap.Push(3);
        minHeap.Push(2);
        minHeap.Push(1);
        minHeap.Push(0);
        minHeap.Push(9);
        minHeap.Push(8);
        minHeap.Push(7);
        minHeap.Push(6);
        minHeap.Push(5);
        Console.WriteLine(minHeap.IsFull);
        while (!minHeap.IsEmpty) Console.WriteLine(minHeap.Pop());
        //myMinHeap.Pop();
    }
}