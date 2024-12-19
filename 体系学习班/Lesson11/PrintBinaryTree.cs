//通过

#region

using System.Text;

#endregion

namespace Algorithms.Lesson11;

public static class PrintBinaryTree
{
    private static void PrintTree(Node head)
    {
        Console.WriteLine("Binary Tree:");
        PrintInOrder(head, 0, "H", 17);
        Console.WriteLine();
    }

    private static void PrintInOrder(Node? head, int height, string to, int len)
    {
        if (head == null) return;

        PrintInOrder(head.Right, height + 1, "v", len);
        var val = to + head.Value + to;
        var lenM = val.Length;
        var lenL = (len - lenM) / 2;
        var lenR = len - lenM - lenL;
        val = GetSpace(lenL) + val + GetSpace(lenR);
        Console.WriteLine(GetSpace(height * len) + val);
        PrintInOrder(head.Left, height + 1, "^", len);
    }

    private static string GetSpace(int num)
    {
        var space = " ";
        var buf = new StringBuilder("");
        for (var i = 0; i < num; i++) buf.Append(space);

        return buf.ToString();
    }

    public static void Run()
    {
        var head = new Node(1)
        {
            Left = new Node(-222222222),
            Right = new Node(3)
        };
        head.Left.Left = new Node(int.MinValue);
        head.Right.Left = new Node(55555555);
        head.Right.Right = new Node(66);
        head.Left.Left.Right = new Node(777);
        PrintTree(head);

        head = new Node(1)
        {
            Left = new Node(2),
            Right = new Node(3)
        };
        head.Left.Left = new Node(4);
        head.Right.Left = new Node(5);
        head.Right.Right = new Node(6);
        head.Left.Left.Right = new Node(7);
        PrintTree(head);

        head = new Node(1)
        {
            Left = new Node(1),
            Right = new Node(1)
        };
        head.Left.Left = new Node(1);
        head.Right.Left = new Node(1);
        head.Right.Right = new Node(1);
        head.Left.Left.Right = new Node(1);
        PrintTree(head);
    }

    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Left;
        public Node? Right;
    }
}