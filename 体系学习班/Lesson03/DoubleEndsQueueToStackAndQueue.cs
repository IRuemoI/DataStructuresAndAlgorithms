// 使用两个栈实现一个队列，并实现其添加、删除、获取栈顶元素的操作。

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson03;

public class DoubleEndsQueueToStackAndQueue
{
    private static bool IsEqual(int? o1, int? o2)
    {
        if (o1 == null && o2 != null) return false;

        if (o1 != null && o2 == null) return false;

        if (o1 == null && o2 == null) return true;

        return o1.Equals(o2);
    }

    public static void Run()
    {
        const int oneTestDataNum = 100;
        const int value = 10000;
        const int testTimes = 100000;

        for (var i = 0; i < testTimes; i++)
        {
            var myStack = new MyStack<int>();
            var myQueue = new MyQueue<int>();
            var stack = new Stack<int>();
            var queue = new Queue<int>();
            for (var j = 0; j < oneTestDataNum; j++)
            {
                var numbers = (int)(Utility.getRandomDouble * value);
                if (stack.Count == 0)
                {
                    myStack.Push(numbers);
                    stack.Push(numbers);
                }
                else
                {
                    if (Utility.getRandomDouble < 0.5)
                    {
                        myStack.Push(numbers);
                        stack.Push(numbers);
                    }
                    else
                    {
                        if (!IsEqual(myStack.Pop(), stack.Pop()))
                            Console.WriteLine("出错啦！");
                    }
                }

                var numQ = (int)(Utility.getRandomDouble * value);
                if (stack.Count == 0)
                {
                    myQueue.Push(numQ);
                    queue.Enqueue(numQ);
                }
                else
                {
                    if (Utility.getRandomDouble < 0.5)
                    {
                        myQueue.Push(numQ);
                        queue.Enqueue(numQ);
                    }
                    else
                    {
                        if (!(myQueue.IsEmpty() && queue.Count == 0) && !IsEqual(myQueue.Poll(), queue.Dequeue()))
                            Console.WriteLine("出错啦！");
                    }
                }
            }
        }

        Console.WriteLine("测试完成");
    }

    private class Node<T>(T data)
    {
        public readonly T Value = data;
        public Node<T>? Last;
        public Node<T>? Next;
    }

    private class DoubleEndsQueue<T>
    {
        private Node<T>? _head;
        private Node<T>? _tail;

        public void AddFromHead(T value)
        {
            var cur = new Node<T>(value);
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

        // public void AddFromBottom(T value)
        // {
        //     var cur = new Node<T>(value);
        //     if (_head == null)
        //     {
        //         _head = cur;
        //         _tail = cur;
        //     }
        //     else
        //     {
        //         cur.Last = _tail;
        //         if (_tail != null) _tail.Next = cur;
        //         _tail = cur;
        //     }
        // }

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
                if (_tail != null)
                    _tail.Next = null;
                if (cur != null)
                    cur.Last = null;
            }

            if (cur != null) return cur.Value;
            throw new InvalidOperationException();
        }

        public bool IsEmpty()
        {
            return _head == null;
        }
    }

    private class MyStack<T>
    {
        private readonly DoubleEndsQueue<T> _queue = new();

        public void Push(T value)
        {
            _queue.AddFromHead(value);
        }

        public T? Pop()
        {
            return _queue.PopFromHead();
        }

        // public bool IsEmpty()
        // {
        //     return _queue.IsEmpty();
        // }
    }

    private class MyQueue<T>
    {
        private readonly DoubleEndsQueue<T> _queue = new();

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
}