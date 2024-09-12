#region

using Common.DataStructures.Stack;
using Common.Utilities;

#endregion

namespace Common.DataStructures.Queue;

public class DoubleLinkedListNode<T>(T data)
{
    public readonly T Value = data;
    public DoubleLinkedListNode<T>? Last;
    public DoubleLinkedListNode<T>? Next;
}

/// <summary>
///     泛型双端队列
/// </summary>
/// <typeparam name="T">泛型</typeparam>
public class CustomDeque<T>
{
    private DoubleLinkedListNode<T>? _head;
    private DoubleLinkedListNode<T>? _tail;

    public void AddFromHead(T value)
    {
        var cur = new DoubleLinkedListNode<T>(value);
        if (_head == null)
        {
            _head = cur;
            _tail = cur;
        }
        else
        {
            cur.Next = _head;
            _head.Last = cur;
            _head = cur;
        }
    }

    public void AddFromBottom(T value)
    {
        var cur = new DoubleLinkedListNode<T>(value);
        if (_head == null)
        {
            _head = cur;
            _tail = cur;
        }
        else
        {
            cur.Last = _tail;
            if (_tail != null) _tail.Next = cur;
            _tail = cur;
        }
    }

    public T? PopFromHead()
    {
        if (_head == null) return default;

        var cur = _head;
        if (_head == _tail)
        {
            _head = null;
            _tail = null;
        }
        else
        {
            _head = _head.Next;
            cur.Next = null;
            if (_head != null) _head.Last = null;
        }

        return cur.Value;
    }

    public T? PopFromBottom()
    {
        if (_head == null) return default;

        var cur = _tail;
        if (_head == _tail)
        {
            _head = null;
            _tail = null;
        }
        else
        {
            _tail = _tail?.Last;
            if (_tail != null) _tail.Next = null;
            if (cur != null) cur.Last = null;
        }

        if (cur != null) return cur.Value;
        throw new AggregateException();
    }

    public bool IsEmpty()
    {
        return _head == null;
    }


    private static bool IsEqual(int? o1, int? o2)
    {
        if (o1 == null && o2 != null) return false;
        if (o1 != null && o2 == null) return false;
        if (o1 == null && o2 == null) return true;
        return o1.Equals(o2);
    }

    public static void Run()
    {
        var oneTestDataNum = 100;
        var value = 100;
        var testTimes = 10;

        for (var i = 0; i < testTimes; i++)
        {
            CustomStack<int> myStack = new();
            CustomQueue<int> myQueue = new();

            Stack<int> stack = new();
            Queue<int> queue = new();

            for (var j = 0; j < oneTestDataNum; j++)
            {
                var numbers = (int)(Utility.GetRandomDouble * value);
                if (stack.Count == 0)
                {
                    myStack.Push(numbers);
                    stack.Push(numbers);
                }
                else
                {
                    if (Utility.GetRandomDouble < 0.5)
                    {
                        myStack.Push(numbers);
                        stack.Push(numbers);
                    }
                    else
                    {
                        if (!IsEqual(myStack.Pop(), stack.Pop())) Console.WriteLine("出错啦！");
                    }
                }

                var numQ = (int)(Utility.GetRandomDouble * value);
                if (stack.Count == 0)
                {
                    myQueue.Push(numQ);
                    queue.Enqueue(numQ);
                }
                else
                {
                    if (Utility.GetRandomDouble < 0.5)
                    {
                        myQueue.Push(numQ);
                        queue.Enqueue(numQ);
                    }
                    else
                    {
                        if (!myQueue.IsEmpty() && queue.Count != 0)
                            if (!IsEqual(myQueue.Poll(), queue.Dequeue()))
                                Console.WriteLine("出错啦！");
                    }
                }
            }
        }

        Console.WriteLine("测试完成");
    }
}