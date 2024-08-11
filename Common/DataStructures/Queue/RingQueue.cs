namespace Common.DataStructures.Queue;

/// <summary>
///     数组实现的循环队列
/// </summary>
public class RingQueue
{
    private readonly int[] _arr;
    private readonly int _limit;
    private int _pollI; // begin
    private int _pushI; // end
    private int _size;

    public RingQueue(int limit)
    {
        _arr = new int[limit];
        _pushI = 0;
        _pollI = 0;
        _size = 0;
        _limit = limit;
    }

    public void Push(int value)
    {
        if (_size == _limit) throw new Exception("Queue is full!");
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
        return i < _limit - 1 ? i + 1 : 0;
    }
}