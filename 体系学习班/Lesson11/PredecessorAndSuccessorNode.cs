//测试通过

namespace Algorithms.Lesson11;

public class PredecessorAndSuccessorNode
{
    private static Node? GetSuccessorNode(Node? node)
    {
        if (node == null) return node;

        if (node.Right != null) return GetLeftMost(node.Right);

        // 无右子树
        var parent = node.Parent;
        while (parent != null && parent.Right == node)
        {
            // 当前节点是其父亲节点右孩子
            node = parent;
            parent = node.Parent;
        }

        return parent;
    }

    private static Node? GetPredecessor(Node? node)
    {
        //todo:待补充
        return null;
    }

    private static Node? GetLeftMost(Node? node)
    {
        if (node == null) return node;

        while (node.Left != null) node = node.Left;

        return node;
    }

    public static void Run()
    {
        var head = new Node(6)
        {
            Parent = null
        };
        head.Left = new Node(3)
        {
            Parent = head,
            Left = new Node(1)
            {
                Parent = head.Left,
                Right = new Node(2)
                {
                    Parent = head.Left?.Left
                }
            },
            Right = new Node(4)
            {
                Parent = head.Left,
                Right = new Node(5)
                {
                    Parent = head.Left?.Right
                }
            }
        };
        head.Right = new Node(9)
        {
            Parent = head,
            Left = new Node(8)
            {
                Parent = head.Right,
                Left = new Node(7)
                {
                    Parent = head.Right?.Left
                }
            },
            Right = new Node(10)
            {
                Parent = head.Right
            }
        };

        var test = head.Left.Left;
        Console.WriteLine(test.Value + " next: " + GetSuccessorNode(test)?.Value);
        test = head.Left.Left.Right;
        Console.WriteLine(test.Value + " next: " + GetSuccessorNode(test)?.Value);
        test = head.Left;
        Console.WriteLine(test.Value + " next: " + GetSuccessorNode(test)?.Value);
        test = head.Left.Right;
        Console.WriteLine(test.Value + " next: " + GetSuccessorNode(test)?.Value);
        test = head.Left.Right.Right;
        Console.WriteLine(test.Value + " next: " + GetSuccessorNode(test)?.Value);
        test = head;
        Console.WriteLine(test.Value + " next: " + GetSuccessorNode(test)?.Value);
        test = head.Right.Left.Left;
        Console.WriteLine(test.Value + " next: " + GetSuccessorNode(test)?.Value);
        test = head.Right.Left;
        Console.WriteLine(test.Value + " next: " + GetSuccessorNode(test)?.Value);
        test = head.Right;
        Console.WriteLine(test.Value + " next: " + GetSuccessorNode(test)?.Value);
        test = head.Right.Right; // 10's next is null
        Console.WriteLine(test.Value + " next: " + GetSuccessorNode(test));
    }

    private class Node(int data)
    {
        public readonly int Value = data;
        public Node? Left;
        public Node? Parent;
        public Node? Right;
    }
}