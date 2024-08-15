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

    //这个能随时获得栈中最小值的结构它所使用的_stackMin内部的数据个数是小于等于_stackData的，
    //只有刚开始时呈递减数据依次入栈时才会是的内部的两个栈的内部的数据个数是相同的；
    //出栈时只有当_stackData顶部的值等于_stackMin顶部的值时才会同时弹出，否则只有_stackData弹出。
    private class MyStack1
    {
        private readonly Stack<int> _stackData = new();
        private readonly Stack<int> _stackMin = new();

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

    //这个能随时获得栈中最小值的结构它所使用的_stackMin内部的数据个数总是等于_stackData的，
    //内部的两个栈时同时入栈和出栈的。
    private class MyStack2
    {
        private readonly Stack<int> _stackData = new();
        private readonly Stack<int> _stackMin = new();

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