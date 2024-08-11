//测试通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson03;

//使用两个队列实现栈
public class TwoQueueImplementStack
{
    public static void Run()
    {
        Console.WriteLine("测试开始");
        TwoQueueStack<int> myStack = new();
        Stack<int> test = new();
        var testTime = 1000000;
        var max = 1000000;

        for (var i = 0; i < testTime; i++)
            if (myStack.IsEmpty())
            {
                if (test.Count != 0) Console.WriteLine("Oops");

                var num = (int)(Utility.GetRandomDouble * max);
                myStack.Push(num);
                test.Push(num);
            }
            else
            {
                if (Utility.GetRandomDouble < 0.25)
                {
                    var num = (int)(Utility.GetRandomDouble * max);
                    myStack.Push(num);
                    test.Push(num);
                }
                else if (Utility.GetRandomDouble < 0.5)
                {
                    if (!myStack.Peek().Equals(test.Peek())) Console.WriteLine("Oops");
                }
                else if (Utility.GetRandomDouble < 0.75)
                {
                    if (!myStack.Dequeue().Equals(test.Pop())) Console.WriteLine("Oops");
                }
                else
                {
                    if (myStack.IsEmpty() != (test.Count == 0)) Console.WriteLine("Oops");
                }
            }

        Console.WriteLine("test finish!");
    }

    private class TwoQueueStack<T>
    {
        private Queue<T> _help;
        private Queue<T> _queue;

        public TwoQueueStack()
        {
            _queue = new Queue<T>();
            _help = new Queue<T>();
        }

        public void Push(T value)
        {
            _queue.Enqueue(value);
        }

        public T Dequeue()
        {
            while (_queue.Count > 1) _help.Enqueue(_queue.Dequeue());

            var ans = _queue.Dequeue();
            (_queue, _help) = (_help, _queue);
            return ans;
        }

        public T Peek()
        {
            while (_queue.Count > 1) _help.Enqueue(_queue.Dequeue());

            var ans = _queue.Dequeue();
            _help.Enqueue(ans);
            (_queue, _help) = (_help, _queue);
            return ans;
        }

        public bool IsEmpty()
        {
            return _queue.Count == 0;
        }
    }
}