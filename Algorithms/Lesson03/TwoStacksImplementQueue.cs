//测试通过

namespace Algorithms.Lesson03;

public class TwoStacksImplementQueue
{
    public static void Run()
    {
        var test = new TwoStacksQueue();
        test.Add(1);
        test.Add(2);
        test.Add(3);
        Console.WriteLine(test.Peek());
        Console.WriteLine(test.Poll());
        Console.WriteLine(test.Peek());
        Console.WriteLine(test.Poll());
        Console.WriteLine(test.Peek());
        Console.WriteLine(test.Poll());
    }

    private class TwoStacksQueue
    {
        private readonly Stack<int> _stackPop;
        private readonly Stack<int> _stackPush;

        public TwoStacksQueue()
        {
            _stackPush = new Stack<int>();
            _stackPop = new Stack<int>();
        }

        // push栈向pop栈倒入数据
        private void PushToPop()
        {
            if (_stackPop.Count == 0)
                while (_stackPush.Count != 0)
                    _stackPop.Push(_stackPush.Pop());
        }

        public void Add(int pushInt)
        {
            _stackPush.Push(pushInt);
            PushToPop();
        }

        public int Poll()
        {
            if (_stackPop.Count == 0 && _stackPush.Count == 0) throw new Exception("Queue is empty!");

            PushToPop();
            return _stackPop.Pop();
        }

        public int Peek()
        {
            if (_stackPop.Count == 0 && _stackPush.Count == 0) throw new Exception("Queue is empty!");

            PushToPop();
            return _stackPop.Peek();
        }
    }
}