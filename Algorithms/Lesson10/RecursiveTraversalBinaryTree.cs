//测试通过

namespace Algorithms.Lesson10;

//递归遍历二叉树
public class RecursiveTraversalBinaryTree
{
    // 先序打印所有节点
    private static void PreOrderTraversal(Node? head)
    {
        if (head == null) return;
        Console.Write(head.Value + " ");
        PreOrderTraversal(head.Left);
        PreOrderTraversal(head.Right);
    }

    private static void InOrderTraversal(Node? head)
    {
        if (head == null) return;
        InOrderTraversal(head.Left);
        Console.Write(head.Value + " ");
        InOrderTraversal(head.Right);
    }

    private static void PostOrderTraversal(Node? head)
    {
        if (head == null) return;
        PostOrderTraversal(head.Left);
        PostOrderTraversal(head.Right);
        Console.Write(head.Value + " ");
    }

    public static void Run()
    {
        var head = new Node(1)
        {
            Left = new Node(2)
            {
                Left = new Node(3),
                Right = new Node(4)
            },
            Right = new Node(5)
            {
                Left = new Node(6),
                Right = new Node(7)
            }
        };

        Console.WriteLine("先序遍历");
        PreOrderTraversal(head);
        Console.WriteLine("\n========");
        Console.WriteLine("中序遍历");
        InOrderTraversal(head);
        Console.WriteLine("\n========");
        Console.WriteLine("后序遍历");
        PostOrderTraversal(head);
        Console.WriteLine("\n========");
    }

    public class Node(int v)
    {
        public readonly int Value = v;
        public Node? Left;
        public Node? Right;
    }
}