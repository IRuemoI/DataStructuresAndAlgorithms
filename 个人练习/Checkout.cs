namespace CustomTraining;

public class Checkout
{
    private readonly LinkedList<int> _queue = new();
    private readonly LinkedList<int> _deque = new();

    public int Get_max()
    {
        return _deque.Count == 0 ? -1 : _queue.First.Value;
    }

    public void Add(int value)
    {
        _queue.AddLast(value);
        while (_deque.Count != 0 && _deque.Last.Value < value) _deque.RemoveLast();

        _deque.AddLast(value);
    }

    public void Remove()
    {
        if (_queue.Count == 0) return;
        if (_queue.First.Value == _deque.First.Value) _deque.RemoveFirst();

        _queue.RemoveFirst();
    }
}