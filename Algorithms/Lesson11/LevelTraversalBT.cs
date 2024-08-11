//通过

namespace Algorithms.Lesson11;

public class LevelTraversalBt
{
    private static void Level(Node? head)
    {
        if (head == null) return;

        Queue<Node> queue = new();
        queue.Enqueue(head);
        while (queue.Count != 0)
        {
            var cur = queue.Dequeue();
            Console.WriteLine(cur.Value);
            if (cur.Left != null) queue.Enqueue(cur.Left);

            if (cur.Right != null) queue.Enqueue(cur.Right);
        }
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
        Level(head);
        Console.WriteLine("========");
    }

    public class Node
    {
        public readonly int Value;
        public Node? Left;
        public Node? Right;

        public Node(int v)
        {
            Value = v;
        }
    }
}