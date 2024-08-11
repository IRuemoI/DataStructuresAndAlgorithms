//测试通过

namespace Algorithms.Lesson10;

public class RecursiveTraversalBt
{
    private static void Function(Node? head)
    {
        if (head == null) return;
        // 1
        Function(head.Left);
        // 2
        Function(head.Right);
        // 3
    }

    // 先序打印所有节点
    private static void Pre(Node? head)
    {
        if (head == null) return;
        Console.WriteLine(head.Value);
        Pre(head.Left);
        Pre(head.Right);
    }

    private static void In1(Node? head)
    {
        if (head == null) return;
        In1(head.Left);
        Console.WriteLine(head.Value);
        In1(head.Right);
    }

    private static void Pos(Node? head)
    {
        if (head == null) return;
        Pos(head.Left);
        Pos(head.Right);
        Console.WriteLine(head.Value);
    }

    public static void Run()
    {
        var head = new Node(1)
        {
            Left = new Node(2),
            Right = new Node(3)
        };
        head.Left.Left = new Node(4);
        head.Left.Right = new Node(5);
        head.Right.Left = new Node(6);
        head.Right.Right = new Node(7);

        Pre(head);
        Console.WriteLine("========");
        In1(head);
        Console.WriteLine("========");
        Pos(head);
        Console.WriteLine("========");
    }

    public class Node(int v)
    {
        public readonly int Value = v;
        public Node? Left;
        public Node? Right;
    }
}