//通过

#region

using Common.Utilities;

#endregion

namespace Algorithms.Lesson12;

public class IsFull
{
    private static bool IsFull1(Node? head)
    {
        if (head == null) return true;

        var height = H(head);
        var nodes = N(head);
        return (1 << height) - 1 == nodes;
    }

    private static int H(Node? head)
    {
        if (head == null) return 0;

        return Math.Max(H(head.Left), H(head.Right)) + 1;
    }

    private static int N(Node? head)
    {
        if (head == null) return 0;

        return N(head.Left) + N(head.Right) + 1;
    }

    private static bool IsFull2(Node? head)
    {
        if (head == null) return true;

        var all = Process(head);
        return (1 << all.Height) - 1 == all.Nodes;
    }

    private static Info Process(Node? head)
    {
        if (head == null) return new Info(0, 0);

        var leftInfo = Process(head.Left);
        var rightInfo = Process(head.Right);
        var height = Math.Max(leftInfo.Height, rightInfo.Height) + 1;
        var nodes = leftInfo.Nodes + rightInfo.Nodes + 1;
        return new Info(height, nodes);
    }

    //���ڲ���
    private static Node? GenerateRandomBst(int maxLevel, int maxValue)
    {
        return Generate(1, maxLevel, maxValue);
    }

    //���ڲ���
    private static Node? Generate(int level, int maxLevel, int maxValue)
    {
        if (level > maxLevel || Utility.GetRandomDouble < 0.5) return null;

        var head = new Node((int)(Utility.GetRandomDouble * maxValue))
        {
            Left = Generate(level + 1, maxLevel, maxValue),
            Right = Generate(level + 1, maxLevel, maxValue)
        };
        return head;
    }

    public static void Run()
    {
        var maxLevel = 5;
        var maxValue = 100;
        var testTimes = 1000000;
        for (var i = 0; i < testTimes; i++)
        {
            var head = GenerateRandomBst(maxLevel, maxValue);
            if (IsFull1(head) != IsFull2(head)) Console.WriteLine("��������");
        }

        Console.WriteLine("测试完成");
    }

    public class Node
    {
        public Node? Left;
        public Node? Right;
        public int Value;

        public Node(int data)
        {
            Value = data;
        }
    }

    private class Info
    {
        public readonly int Height;
        public readonly int Nodes;

        public Info(int h, int n)
        {
            Height = h;
            Nodes = n;
        }
    }
}