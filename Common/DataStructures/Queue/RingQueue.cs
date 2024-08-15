namespace Common.DataStructures.Queue;

/// <summary>
///     数组实现的循环队列
/// </summary>
public class RingQueue(int limit)
{
    private readonly int[] _arr = new int[limit];
    private int _pollI; // begin
    private int _pushI; // end
    private int _size;

    public void Push(int value)
    {
        if (_size == limit) throw new Exception("Queue is full!");
        _size++;
        _arr[_pushI] = value;
        _pushI = NextIndex(_pushI);
    }

    public int Pop()
    {
        if (_size == 0) throw new Exception("Queue is empty!");
        _size--;
        var ans = _arr[_pollI];
        _pollI = NextIndex(_pollI);
        return ans;
    }

    public bool IsEmpty()
    {
        return _size == 0;
    }

    // 如果现在的下标是i，返回下一个位置
    private int NextIndex(int i)
    {
        return i < limit - 1 ? i + 1 : 0;
    }
}