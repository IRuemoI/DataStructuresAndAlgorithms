//测试通过

namespace Algorithms.Lesson10;

//非递归遍历二叉树
public class UnRecursiveTraversalBinaryTree
{
    private static void PreOrderTraversal(Node? head)
    {
        Console.Write("pre-order: ");
        if (head != null)
        {
            var stack = new Stack<Node>();
            stack.Push(head);
            while (stack.Count != 0)
            {
                head = stack.Pop();
                Console.Write(head.Value + " ");
                if (head.Right != null) stack.Push(head.Right);

                if (head.Left != null) stack.Push(head.Left);
            }
        }

        Console.WriteLine();
    }

    private static void InOrderTraversal(Node? cur)
    {
        Console.Write("in-order: ");
        if (cur != null)
        {
            var stack = new Stack<Node>();
            while (stack.Count != 0 || cur != null)
                if (cur != null)
                {
                    stack.Push(cur);
                    cur = cur.Left;
                }
                else
                {
                    cur = stack.Pop();
                    Console.Write(cur.Value + " ");
                    cur = cur.Right;
                }
        }

        Console.WriteLine();
    }

    private static void PostOrderTraversal(Node? head)
    {
        Console.Write("pos-order: ");
        if (head != null)
        {
            var s1 = new Stack<Node>();
            var s2 = new Stack<Node>();
            s1.Push(head);
            while (s1.Count != 0)
            {
                head = s1.Pop(); // 头 右 左
                s2.Push(head);
                if (head.Left != null) s1.Push(head.Left);

                if (head.Right != null) s1.Push(head.Right);
            }

            // 左 右 头
            while (s2.Count != 0) Console.Write(s2.Pop().Value + " ");
        }

        Console.WriteLine();
    }

    private static void PostOrderTraversal2(Node? h)
    {
        Console.Write("pos-order: ");
        if (h != null)
        {
            var stack = new Stack<Node>();
            stack.Push(h);
            while (stack.Count != 0)
            {
                var c = stack.Peek();
                if (c.Left != null && h != c.Left && h != c.Right)
                {
                    stack.Push(c.Left);
                }
                else if (c.Right != null && h != c.Right)
                {
                    stack.Push(c.Right);
                }
                else
                {
                    Console.Write(stack.Pop().Value + " ");
                    h = c;
                }
            }
        }

        Console.WriteLine();
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
        
        PreOrderTraversal(head);
        Console.WriteLine("========");
        InOrderTraversal(head);
        Console.WriteLine("========");
        PostOrderTraversal(head);
        Console.WriteLine("========");
        PostOrderTraversal2(head);
        Console.WriteLine("========");
    }

    public class Node(int v)
    {
        public readonly int Value = v;
        public Node? Left;
        public Node? Right;
    }
}