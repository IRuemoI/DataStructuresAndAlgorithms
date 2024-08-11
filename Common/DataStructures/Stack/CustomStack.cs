#region

using Common.DataStructures.Queue;

#endregion

namespace Common.DataStructures.Stack;

public class CustomStack<T>
{
    private readonly CustomDeque<T> _queue;

    public CustomStack()
    {
        _queue = new CustomDeque<T>();
    }

    public void Push(T value)
    {
        _queue.AddFromHead(value);
    }

    public T? Pop()
    {
        return _queue.PopFromHead();
    }

    public bool IsEmpty()
    {
        return _queue.IsEmpty();
    }
}