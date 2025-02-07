namespace Algorithms.Lesson03;

// 使用数组实现一个环形队列，并实现相应的入队和出队操作。
public class RingArray(int limit)
{
    private readonly int[] _arr = new int[limit];
    private int _pollIndex; // begin
    private int _pushIndex; // end
    private int _size;

    public void Push(int value)
    {
        if (_size == limit) throw new Exception("Queue is full!");

        _size++;
        _arr[_pushIndex] = value;
        _pushIndex = NextIndex(_pushIndex);
    }

    public int Pop()
    {
        if (_size == 0) throw new Exception("Queue is empty!");

        _size--;
        var ans = _arr[_pollIndex];
        _pollIndex = NextIndex(_pollIndex);
        return ans;
    }

    public bool IsEmpty()
    {
        return _size == 0;
    }

    public bool IsFull()
    {
        return _size == limit;
    }

    // 如果现在的下标是i，返回下一个位置
    private int NextIndex(int i)
    {
        return i < limit - 1 ? i + 1 : 0;
    }
}

public class RingArrayTest
{
    public static void Run()
    {
        var ringArray = new RingArray(3);
        Console.WriteLine(ringArray.IsEmpty());
        ringArray.Push(1);
        ringArray.Push(2);
        ringArray.Push(3);
        Console.WriteLine(ringArray.IsFull());
        //ringArray.Push(4);
        Console.WriteLine(ringArray.Pop());
    }
}