namespace Common.DataStructures.Queue;

/// <summary>
///     泛型队列
/// </summary>
/// <typeparam name="T">泛型</typeparam>
public class CustomQueue<T>
{
    private readonly CustomDeque<T> _queue = new();

    public void Push(T value)
    {
        _queue.AddFromHead(value);
    }

    public T? Poll()
    {
        return _queue.PopFromBottom();
    }

    public bool IsEmpty()
    {
        return _queue.IsEmpty();
    }
}