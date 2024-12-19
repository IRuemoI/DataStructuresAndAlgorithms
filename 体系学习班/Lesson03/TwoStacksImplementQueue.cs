//测试通过

namespace Algorithms.Lesson03;

//用两个栈模拟队列的思路：数据整体入栈出栈后会导致顺序颠倒，只要颠倒两次就可以保持顺序
public class TwoStacksImplementQueue
{
    public static void Run()
    {
        var test = new TwoStacksQueue();
        test.Push(1);
        test.Push(2);
        test.Push(3);
        Console.WriteLine(test.Peek());
        Console.WriteLine(test.Poll());
        Console.WriteLine(test.Peek());
        Console.WriteLine(test.Poll());
        Console.WriteLine(test.Peek());
        Console.WriteLine(test.Poll());
    }

    private class TwoStacksQueue
    {
        private readonly Stack<int> _stackPop = new();
        private readonly Stack<int> _stackPush = new();

        // push栈向pop栈倒入数据
        private void PushToPop()
        {
            if (_stackPop.Count == 0)
                while (_stackPush.Count != 0)
                    _stackPop.Push(_stackPush.Pop());
        }

        public void Push(int pushInt)
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