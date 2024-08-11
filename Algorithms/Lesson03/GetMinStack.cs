//通过

namespace Algorithms.Lesson03;

public class GetMinStack
{
    public static void Run()
    {
        var stack1 = new MyStack1();
        stack1.Push(3);
        Console.WriteLine(stack1.GetMin());
        stack1.Push(4);
        Console.WriteLine(stack1.GetMin());
        stack1.Push(1);
        Console.WriteLine(stack1.GetMin());
        Console.WriteLine(stack1.Pop());
        Console.WriteLine(stack1.GetMin());

        Console.WriteLine("=============");

        var stack2 = new MyStack2();
        stack2.Push(3);
        Console.WriteLine(stack2.GetMin());
        stack2.Push(4);
        Console.WriteLine(stack2.GetMin());
        stack2.Push(1);
        Console.WriteLine(stack2.GetMin());
        Console.WriteLine(stack2.Pop());
        Console.WriteLine(stack2.GetMin());
    }

    private class MyStack1
    {
        private readonly Stack<int> _stackData;
        private readonly Stack<int> _stackMin;

        public MyStack1()
        {
            _stackData = new Stack<int>();
            _stackMin = new Stack<int>();
        }

        public void Push(int newNum)
        {
            if (_stackMin.Count == 0)
                _stackMin.Push(newNum);
            else if (newNum <= GetMin()) _stackMin.Push(newNum);

            _stackData.Push(newNum);
        }

        public int Pop()
        {
            if (_stackData.Count == 0) throw new Exception("Your stack is empty.");

            var value = _stackData.Pop();
            if (value == GetMin()) _stackMin.Pop();

            return value;
        }

        public int GetMin()
        {
            if (_stackMin.Count == 0) throw new Exception("Your stack is empty.");

            return _stackMin.Peek();
        }
    }

    private class MyStack2
    {
        private readonly Stack<int> _stackData;
        private readonly Stack<int> _stackMin;

        public MyStack2()
        {
            _stackData = new Stack<int>();
            _stackMin = new Stack<int>();
        }

        public void Push(int newNum)
        {
            if (_stackMin.Count == 0)
            {
                _stackMin.Push(newNum);
            }
            else if (newNum < GetMin())
            {
                _stackMin.Push(newNum);
            }
            else
            {
                var newMin = _stackMin.Peek();
                _stackMin.Push(newMin);
            }

            _stackData.Push(newNum);
        }

        public int Pop()
        {
            if (_stackData.Count == 0) throw new Exception("Your stack is empty.");

            _stackMin.Pop();
            return _stackData.Pop();
        }

        public int GetMin()
        {
            if (_stackMin.Count == 0) throw new Exception("Your stack is empty.");

            return _stackMin.Peek();
        }
    }
}