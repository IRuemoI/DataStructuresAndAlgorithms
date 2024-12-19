//测试通过

namespace Algorithms.Lesson17;

public class Hanoi
{
    private static void Hanoi1(int n)
    {
        LeftToRight(n);
    }

    // 请把1~N层圆盘 从左 -> 右
    private static void LeftToRight(int n)
    {
        if (n == 1)
        {
            // base case
            Console.WriteLine("Move 1 from left to right");
            return;
        }

        LeftToMid(n - 1);
        Console.WriteLine("Move " + n + " from left to right");
        MidToRight(n - 1);
    }

    // 请把1~N层圆盘 从左 -> 中
    private static void LeftToMid(int n)
    {
        if (n == 1)
        {
            Console.WriteLine("Move 1 from left to mid");
            return;
        }

        LeftToRight(n - 1);
        Console.WriteLine("Move " + n + " from left to mid");
        RightToMid(n - 1);
    }

    private static void RightToMid(int n)
    {
        if (n == 1)
        {
            Console.WriteLine("Move 1 from right to mid");
            return;
        }

        RightToLeft(n - 1);
        Console.WriteLine("Move " + n + " from right to mid");
        LeftToMid(n - 1);
    }

    private static void MidToRight(int n)
    {
        if (n == 1)
        {
            Console.WriteLine("Move 1 from mid to right");
            return;
        }

        MidToLeft(n - 1);
        Console.WriteLine("Move " + n + " from mid to right");
        LeftToRight(n - 1);
    }

    private static void MidToLeft(int n)
    {
        if (n == 1)
        {
            Console.WriteLine("Move 1 from mid to left");
            return;
        }

        MidToRight(n - 1);
        Console.WriteLine("Move " + n + " from mid to left");
        RightToLeft(n - 1);
    }

    private static void RightToLeft(int n)
    {
        if (n == 1)
        {
            Console.WriteLine("Move 1 from right to left");
            return;
        }

        RightToMid(n - 1);
        Console.WriteLine("Move " + n + " from right to left");
        MidToLeft(n - 1);
    }

    private static void Hanoi2(int n)
    {
        if (n > 0) Func(n, "left", "right", "mid");
    }

    private static void Func(int n, string from, string to, string other)
    {
        if (n == 1)
        {
            // base
            Console.WriteLine("Move 1 from " + from + " to " + to);
        }
        else
        {
            Func(n - 1, from, other, to);
            Console.WriteLine("Move " + n + " from " + from + " to " + to);
            Func(n - 1, other, to, from);
        }
    }

    private static void Hanoi3(int n)
    {
        if (n < 1) return;

        Stack<Record> stack = new();
        stack.Push(new Record(false, n, "left", "right", "mid"));
        while (stack.Count != 0)
        {
            var cur = stack.Pop();
            if (cur.Basic == 1)
            {
                Console.WriteLine("Move 1 from " + cur.From + " to " + cur.To);
                if (stack.Count != 0) stack.Peek().Finish1 = true;
            }
            else
            {
                if (!cur.Finish1)
                {
                    stack.Push(cur);
                    stack.Push(new Record(false, cur.Basic - 1, cur.From, cur.Other, cur.To));
                }
                else
                {
                    Console.WriteLine("Move " + cur.Basic + " from " + cur.From + " to " + cur.To);
                    stack.Push(new Record(false, cur.Basic - 1, cur.Other, cur.To, cur.From));
                }
            }
        }
    }

    public static void Run()
    {
        var n = 3;
        Hanoi1(n);
        Console.WriteLine("============");
        Hanoi2(n);
        Console.WriteLine("============");
        Hanoi3(n);
    }

    private class Record
    {
        public readonly int Basic;
        public readonly string From;
        public readonly string Other;
        public readonly string To;
        public bool Finish1;

        public Record(bool f1, int b, string f, string t, string o)
        {
            Finish1 = f1;
            Basic = b;
            From = f;
            To = t;
            Other = o;
        }
    }
}